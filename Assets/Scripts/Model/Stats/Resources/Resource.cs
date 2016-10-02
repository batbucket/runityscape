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
    public const int GREATER_CAP = 99999;

    public override float False { get { return Mathf.Clamp(base.False, LESSER_CAP, True); } set { base.False = value; } }
    public override int True { get { return Mathf.Clamp(base.True, LESSER_CAP, GREATER_CAP); } set { base.True = value; } }

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
