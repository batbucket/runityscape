using UnityEngine;
using System.Collections;

/**
 * This class represents a Resource
 * Think health, mana, etc.
 *
 * These values have a lesser cap of 0
 */
public class Resource : PairedInt {
    public const int LESSER_CAP = 0;

    public override void setTrue(int trueValue) {
        base.setTrue(Mathf.Max(trueValue, LESSER_CAP));
    }

    public override void setFalse(int falseValue) {
        base.setTrue(Mathf.Max(falseValue, LESSER_CAP));
    }

    public void clearFalse() {
        setFalse(LESSER_CAP);
    }
}
