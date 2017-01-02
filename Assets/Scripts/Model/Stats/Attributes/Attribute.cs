using UnityEngine;

/**
 * This class represents a certain stat
 * Think strength, agility, intelligence
 *
 * These values have a lesser and greater cap
 */
public abstract class Attribute : PairedInt {
    readonly AttributeType _type;
    public AttributeType Type { get { return _type; } }

    public const int LESSER_CAP = 1;
    public const int GREATER_CAP = 999;

    public override sealed float False { get { return Mathf.Clamp(base.False, LESSER_CAP, GREATER_CAP); } set { base.False = value; } }
    public override sealed int True { get { return Mathf.Clamp(base.True, LESSER_CAP, GREATER_CAP); } set { base.True = value; } }

    public Attribute(int initial, AttributeType type) : base(initial) {
        this._type = type;
    }
}
