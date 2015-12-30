using UnityEngine;
using System.Collections;

/**
 * This class represents a certain stat
 * Think strength, agility, intelligence
 *
 * These values have a lesser and greater cap
 */
public abstract class Attribute : PairedInt {
    public const int LESSER_CAP = 1;
    public const int GREATER_CAP = 999999;

    public override void setTrue(int trueValue) {
        base.setTrue(Mathf.Clamp(trueValue, LESSER_CAP, GREATER_CAP));
    }

    public override void setFalse(int falseValue) {
        base.setFalse(Mathf.Clamp(falseValue, LESSER_CAP, GREATER_CAP));
    }
}
