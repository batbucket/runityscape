using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/**
 * Spells always consume 100% of the user's charge bar
 * which is not considered to be a cost as it varies
 */
public abstract class SpellFactory {

    readonly IDictionary<SpellResult.Type, SpellResult> results;

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

        this.results = new SortedDictionary<SpellResult.Type, SpellResult>() {
            { SpellResult.Type.MISS, new SpellResult() {        isState         = (s) => !IsHit(s),
                                                                calculateEffect = OnMissCalculation,
                                                                doEffect        = OnMiss,
                                                                createText      = OnMissText,
                                                                undoEffect      = OnMissUndo,
                                                                sfx             = OnMissSFX } },

            { SpellResult.Type.CRITICAL, new SpellResult() {    isState         = IsCritical,
                                                                calculateEffect = OnCriticalCalculation,
                                                                doEffect        = OnCritical,
                                                                createText      = OnCriticalText,
                                                                undoEffect      = OnCriticalUndo,
                                                                sfx             = OnCriticalSFX } },

            { SpellResult.Type.HIT, new SpellResult() {         isState         = (s) => true,
                                                                calculateEffect = OnHitCalculation,
                                                                doEffect        = OnHit,
                                                                createText      = OnHitText,
                                                                undoEffect      = OnHitUndo,
                                                                sfx             = OnHitSFX } },
        };
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
                Cast(caster, target, new Spell(this, caster, target));
            }
        }
    }

    void Cast(Character caster, Character target, Spell spell) {
        SpellResult res = DetermineResult(spell);

        res.calculateEffect(spell);
        spell.CastText = res.createText(spell);
        spell.Process.Action = () => {
            res.doEffect(spell);
            Game.Instance.TextBoxHolder.AddTextBoxView(new TextBox(res.createText(spell), Color.white, TextEffect.FADE_IN));
            res.sfx(spell);
        };
        spell.Process.UndoAction = () => res.undoEffect(spell);

        caster.CastSpells.Add(spell);
        target.RecievedSpells.Add(spell);
        target.React(spell);

        foreach (Character c in Game.Instance.PagePresenter.Page.GetAll()) {
            c.Witness(spell);
        }
    }

    SpellResult DetermineResult(Spell spell) {
        foreach (KeyValuePair<SpellResult.Type, SpellResult> pair in results) {
            if (pair.Value.isState(spell)) {
                spell.Result = pair.Key;
                return pair.Value;
            }
        }
        throw new UnityException("Iterated through results Dictionary and no isState evaluated to true!");
    }

    public override bool Equals(object obj) {
        // If parameter is null return false.
        if (obj == null) {
            return false;
        }

        // If parameter cannot be cast to Page return false.
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

    //Helper methods to make SpellFactories easier to make

    public virtual bool IsHit(Spell spell) { return true; }
    protected abstract void OnHitCalculation(Spell spell);
    protected virtual void OnHit(Spell spell) { spell.NumericPlay(); }
    protected abstract string OnHitText(Spell spell);
    protected virtual void OnHitUndo(Spell spell) { Undo(spell); }
    protected virtual void OnHitSFX(Spell spell) { }

    protected virtual void OnMissCalculation(Spell spell) { }
    protected virtual void OnMiss(Spell spell) { spell.NumericPlay(); }
    protected virtual string OnMissText(Spell spell) { return "MISS TEXT NOT IMPLEMENTED."; }
    protected virtual void OnMissUndo(Spell spell) { Undo(spell); }
    protected virtual void OnMissSFX(Spell spell) { }

    public virtual bool IsCritical(Spell spell) { return false; }
    protected virtual void OnCriticalCalculation(Spell spell) { }
    protected virtual void OnCritical(Spell spell) { spell.NumericPlay(); }
    protected virtual string OnCriticalText(Spell spell) { return "CRITICAL TEXT NOT IMPLEMENTED."; }
    protected virtual void OnCriticalUndo(Spell spell) { Undo(spell); }
    protected virtual void OnCriticalSFX(Spell spell) { }

    protected virtual void OnOnce(Character caster) { }

    public virtual void Undo(Spell spell) { spell.NumericUndo(); }
}