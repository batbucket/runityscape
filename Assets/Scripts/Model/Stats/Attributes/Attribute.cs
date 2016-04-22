using UnityEngine;

/**
 * This class represents a certain stat
 * Think strength, agility, intelligence
 *
 * These values have a lesser and greater cap
 */
public class Attribute : PairedInt {
    public const int LESSER_CAP = 1;
    public const int GREATER_CAP = 999;

    public override float False { get { return base.False; } set { base.False = Mathf.Clamp(value, LESSER_CAP, GREATER_CAP); } }
    public override int True { get { return base.True; } set { base.True = Mathf.Clamp(value, LESSER_CAP, GREATER_CAP); } }

    public Attribute(int initial) : base(initial) { }
}
