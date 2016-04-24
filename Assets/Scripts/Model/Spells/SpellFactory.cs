using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/**
 * Spells always consume 100% of the user's charge bar
 * which is not considered to be a cost as it varies
 */
public abstract class SpellFactory {

    string _name;
    public string Name { get { return _name; } }

    string _description;
    public string Description { get { return _description; } }

    readonly SpellType _spellType;
    public SpellType SpellType { get { return _spellType; } }

    readonly TargetType _targetType;
    public TargetType TargetType { get { return _targetType; } }
    public bool IsEnabled { get; set; }

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

    public SpellFactory(string name,
                        string description,
                        SpellType spellType,
                        TargetType targetType,
                        Dictionary<ResourceType, int> costs) {

        this._name = name;
        this._description = description;
        this._spellType = spellType;
        this._targetType = targetType;
        this.costs = costs;
        this.IsEnabled = true;
    }

    public virtual string GetNameAndInfo(Character caster) {
        StringBuilder s = new StringBuilder();
        s.Append((IsCastable(caster) ? Name : Util.Color(Name, Color.red)) + (costs.Count == 0 ? "" : " - "));
        List<string> elements = new List<string>();
        foreach (KeyValuePair<ResourceType, int> entry in costs) {
            if (entry.Key != ResourceType.CHARGE) {
                Color resourceColor = entry.Key.FillColor;
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
        if (!caster.IsCharged() || !IsEnabled) {
            return false;
        }
        foreach (KeyValuePair<ResourceType, int> resourceCost in costs) {
            if (!caster.Resources.ContainsKey(resourceCost.Key) || caster.GetResourceCount(resourceCost.Key, false) < resourceCost.Value) {
                return false;
            }
        }
        return true;
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
            OnOnce(caster);
            foreach (Character target in targets) {
                Cast(caster, target);
            }
        }
    }

    public const string PRIMARY = "Primary_Component";
    void Cast(Character caster, Character target) {
        Spell spell = new Spell(this, caster, target);
        IDictionary<string, SpellComponent> components = CreateComponents(caster, target, spell);
        spell.Components = components;

        caster.CastSpells.Add(spell);
        target.RecievedSpells.Add(spell);
        target.AddToBuffs(spell);
    }

    public bool IsSingleTargetQuickCastable(Character caster, IList<Character> targets) {
        return targets.Count == 1;
    }

    public override bool Equals(object obj) {
        // If parameter is null return false.
        if (obj == null) {
            return false;
        }

        // If parameter cannot be cast to SpellFactory return false.
        SpellFactory s = obj as SpellFactory;
        if ((object)s == null) {
            return false;
        }

        // Return true if the fields match:
        return this.Name.Equals(s.Name);
    }

    public override int GetHashCode() {
        return Name.GetHashCode();
    }

    protected virtual void OnOnce(Character caster) { }
    protected abstract IDictionary<string, SpellComponent> CreateComponents(Character caster, Character target, Spell spell);
}