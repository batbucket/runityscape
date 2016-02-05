using UnityEngine;

/**
 * This class represents a Resource
 * Think health, mana, etc.
 *
 * These values have a lesser cap of 0
 * and a variable greater cap
 */
public abstract class Resource : PairedInt {

    public const int LESSER_CAP = 0;

    public override int True { get { return base.True; } set { base.True = Mathf.Clamp(value, LESSER_CAP, 99999); } }
    public override int False { get { return base.False; } set { base.False = Mathf.Clamp(value, LESSER_CAP, True); } }

    public void clearFalse() {
        False = LESSER_CAP;
    }

    public abstract Color OverColor { get; }
    public abstract Color UnderColor { get; }
    public abstract string Name { get; }
    public abstract string ShortName { get; }
    public abstract string Description { get; }
    public abstract ResourceType Type { get; }

    public virtual void regenerate() { }

    public virtual void regenerate(int amount) {
        False += amount;
    }
}
