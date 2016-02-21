using UnityEngine;

/**
 * This class represents a Resource
 * Think health, mana, etc.
 *
 * These values have a lesser cap of 0
 * and a variable greater cap
 */
public class Resource : PairedInt {

    public const int LESSER_CAP = 0;

    public override int True { get { return base.True; } set { base.True = Mathf.Clamp(value, LESSER_CAP, 99999); } }
    public override int False { get { return base.False; } set { base.False = Mathf.Clamp(value, LESSER_CAP, True); } }

    public Resource(int initial) : base(initial) { }

    public void ClearFalse() {
        False = LESSER_CAP;
    }

    public virtual void regenerate() { }

    public virtual void regenerate(int amount) {
        False += amount;
    }
}
