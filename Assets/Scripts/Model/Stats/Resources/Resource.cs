using UnityEngine;

/**
 * This class represents a Resource
 * Think health, mana, etc.
 *
 * These values have a lesser cap of 0
 * and a variable greater cap
 */
public abstract class Resource : PairedInt {
    public bool IsVisible { get; set; }

    readonly ResourceType _type;
    public ResourceType Type { get { return _type; } }

    public const int LESSER_CAP = 0;

    public override float False { get { return base.False; } set { base.False = Mathf.Clamp(value, LESSER_CAP, True); } }
    public override int True { get { return base.True; } set { base.True = Mathf.Clamp(value, LESSER_CAP, 99999); } }

    public Resource(int initial, ResourceType type) : base(initial) {
        this._type = type;
        this.IsVisible = true;
    }

    public Resource(int trueValue, int falseValue, ResourceType type) : base(trueValue, falseValue) {
        this._type = type;
        this.IsVisible = true;
    }

    public virtual void Calculate() { }
}
