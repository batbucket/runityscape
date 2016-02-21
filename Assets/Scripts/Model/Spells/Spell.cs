using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

/**
 * Spells always consume 100% of the user's charge bar
 * which is not considered to be a cost as it varies
 */
public abstract class Spell : ICloneable, IUndoableProcess {
    public string Name { get; private set; }
    public string Description { get; private set; }
    public SpellType SpellType { get; private set; }
    public SpellTarget TargetType { get; private set; }
    public bool IsEnabled { get; set; }
    public string CastText { get; set; }
    public SpellResult Result { get; private set; }
    public Character Caster { get; private set; }
    public Character Target { get; private set; }
    public int Amount { get; protected set; }

    /**
     * Inventory needs to be in the Character's Selections
     * Selections is an ICollection<Selection, ICollection<Spell>>
     * So we need to implement ICollection<Spell> in Inventory
     * The only field that differentiates a Spell from an Item
     * Is the Count field.
     * If we keep this here, we can implement ICollection<Spell>
     * In inventory, and force the caller to only add Items to the collection.
     * This is way better than the alternative.
     */
    private int _count;
    public int Count {
        get {
            return _count;
        }
        set {
            Util.Assert(value >= 0);
            _count = value;
        }
    }
    Dictionary<ResourceType, int> costs;

    Action IUndoableProcess.UndoAction { get { return new Action(() => Undo()); } }
    Action IProcess.Action { get { return new Action(() => OnSuccess(Caster, Target)); } }

    string successSound;
    string failureSound;

    public Spell(string name, string description, SpellType spellType, SpellTarget targetType, Dictionary<ResourceType, int> costs) {
        this.Name = name;
        this.Description = description;
        this.SpellType = spellType;
        this.TargetType = targetType;
        this.costs = costs;
        this.IsEnabled = true;
        this.successSound = successSound;
        this.failureSound = failureSound;
    }

    public Spell(string name, SpellType spellType, SpellTarget targetType) {
        this.Name = name;
        this.SpellType = spellType;
        this.TargetType = targetType;
        this.costs = new Dictionary<ResourceType, int>();
    }

    public virtual string GetNameAndInfo(Character caster) {
        StringBuilder s = new StringBuilder();
        s.Append((IsCastable(caster) ? Name : Util.Color(Name, Color.red)) + (costs.Count == 0 ? "" : " - "));
        List<string> elements = new List<string>();
        foreach (KeyValuePair<ResourceType, int> entry in costs) {
            if (entry.Key != ResourceType.CHARGE) {
                Color resourceColor = entry.Key.OverColor;
                int cost = entry.Value;
                elements.Add(Util.Color("" + cost, resourceColor));
            }
        }
        s.Append(string.Join("/", elements.ToArray()));
        return s.ToString();
    }

    public virtual bool IsCastable(Character caster, Character target = null) {
        if (target != null && !target.IsTargetable) {
            return false;
        }
        foreach (KeyValuePair<ResourceType, int> resourceCost in costs) {
            if (!caster.Resources.ContainsKey(resourceCost.Key) || caster.GetResourceCount(resourceCost.Key, false) < resourceCost.Value) {
                return false;
            }
        }
        return caster.IsCharged();
    }

    protected virtual void ConsumeResources(Character caster) {
        foreach (KeyValuePair<ResourceType, int> resourceCost in costs) {
            caster.AddToResource(resourceCost.Key, false, -resourceCost.Value);
        }
        caster.Discharge();
    }

    public void TryCast(Character caster, Character target) {
        TryCast(caster, new List<Character>() { target });
    }

    public void TryCast(Character caster, IList<Character> targets) {
        if (IsCastable(caster)) {
            ConsumeResources(caster);
            foreach (Character target in targets) {
                Cast(caster, target);
            }
        } else {
            Result = SpellResult.CANT_CAST;
        }
    }

    void Cast(Character caster, Character target) {
        OnAny(caster, target);
        if (Util.Chance(CalculateHitRate(caster, target))) {
            OnHit(caster, target);
            Result = SpellResult.HIT;
        } else {
            Game.Instance.Effect.CreateHitsplat("MISS", Color.grey, target);
            OnMiss(caster, target);
            Result = SpellResult.MISS;
        }
        this.Caster = caster;
        this.Target = target;
        caster.AddToCastSpellHistory((Spell)Clone());
        target.AddToRecievedSpellHistory((Spell)Clone());
    }

    public override bool Equals(object obj) {
        // If parameter is null return false.
        if (obj == null) {
            return false;
        }

        // If parameter cannot be cast to Page return false.
        Spell s = obj as Spell;
        if ((object)s == null) {
            return false;
        }

        // Return true if the fields match:
        return this.Name.Equals(s.Name);
    }

    public override int GetHashCode() {
        return Name.GetHashCode();
    }

    public object Clone() {
        return this.MemberwiseClone();
    }

    void IProcess.Play() {
        OnSuccess(Caster, Target);
    }

    void IUndoableProcess.Undo() {
        Undo();
    }

    public abstract double CalculateHitRate(Character caster, Character target);
    public abstract int CalculateAmount(Character caster, Character target);
    protected virtual void OnAny(Character caster, Character target) { }
    protected abstract void OnHit(Character caster, Character target);
    protected abstract void OnSuccess(Character caster, Character target);
    protected abstract void OnFailure(Character caster, Character target);
    protected abstract void OnMiss(Character caster, Character target);
    public abstract void Undo();
}
