using UnityEngine;

/**
 * This class represents a Resource
 * Think health, mana, etc.
 *
 * These values have a lesser cap of 0
 * and a variable greater cap
 */
public abstract class Resource : PairedInt {
    public AttributeType Dependent;

    public bool IsVisible { get; set; }

    readonly ResourceType _type;
    public ResourceType Type { get { return _type; } }

    public const int LESSER_CAP = 0;
    public const int GREATER_CAP = 99999;

    private readonly bool isFalseCappedAtTrue;

    public override float False {
        get {
            return Mathf.Clamp(base.False, LESSER_CAP, isFalseCappedAtTrue ? True : GREATER_CAP);
        }
        set {
            base.False = value;
        }
    }

    public override int True {
        get {
            return Mathf.Clamp(base.True, LESSER_CAP, GREATER_CAP);
        }
        set {
            base.True = value;
        }
    }

    public Resource(int trueValue, int falseValue, ResourceType type, bool isFalseCappedAtTrue) : base(trueValue, falseValue) {
        this._type = type;
        this.IsVisible = true;
        this.isFalseCappedAtTrue = isFalseCappedAtTrue;
    }

    public Resource(int initial, ResourceType type, bool isFalseCappedAtTrue) : this(initial, initial, type, isFalseCappedAtTrue) { }

    public virtual void Calculate(int attribute) {

    }
}
