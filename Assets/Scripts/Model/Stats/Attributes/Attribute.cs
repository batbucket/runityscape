using UnityEngine;

/**
 * This class represents a certain stat
 * Think strength, agility, intelligence
 *
 * These values have a lesser and greater cap
 */
public abstract class Attribute : PairedInt {
    public const int LESSER_CAP = 1;
    public const int GREATER_CAP = 999;

    public override int True { get { return base.True; } set { base.True = Mathf.Clamp(value, LESSER_CAP, GREATER_CAP); } }
    public override int False { get { return base.False; } set { base.False = Mathf.Clamp(value, LESSER_CAP, GREATER_CAP); } }
    public abstract string Name { get; }
    public abstract string ShortName { get; }
    public abstract string PrimaryDescription { get; }
    public abstract string SecondaryDescription { get; }
    public abstract string ShortDescription { get; }
    public abstract AttributeType Type { get; }
    public abstract Color Color { get; }
}
