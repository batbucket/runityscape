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

    public override sealed float False { get { return (Flat + base.False) * Percent; } set { base.False = Mathf.Clamp(value, LESSER_CAP, GREATER_CAP); } }
    public override sealed int True { get { return base.True; } set { base.True = Mathf.Clamp(value, LESSER_CAP, GREATER_CAP); } }

    //Bonuses
    int _flat;
    float _percent;

    public override sealed int Flat { get { return _flat; } set { _flat = value; } }
    public override sealed float Percent { get { return _percent; } set { Mathf.Max(0, _percent = value); } }

    public Attribute(int initial, AttributeType type) : base(initial) {
        this._type = type;
        this._percent = 1;
    }
}
