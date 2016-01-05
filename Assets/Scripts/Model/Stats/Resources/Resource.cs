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

    public override void setTrue(int trueValue) {
        base.setTrue(Mathf.Clamp(trueValue, LESSER_CAP, trueValue));
    }

    public override void setFalse(int falseValue) {
        base.setFalse(Mathf.Clamp(falseValue, LESSER_CAP, getTrue()));
    }

    public void clearFalse() {
        setFalse(LESSER_CAP);
    }

    public abstract Color getOverColor();
    public abstract Color getUnderColor();
    public abstract string getLongName();
    public abstract string getShortName();
    public abstract string getDescription();
    public abstract ResourceType getResourceType();

    public virtual void regenerate() {

    }

    public virtual void regenerate(int amount) {
        addFalse(amount);
    }
}
