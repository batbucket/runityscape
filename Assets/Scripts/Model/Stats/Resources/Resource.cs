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
    int greaterCap;

    public override void setTrue(int trueValue) {
        base.setTrue(Mathf.Clamp(trueValue, LESSER_CAP, greaterCap));
    }

    public override void setFalse(int falseValue) {
        base.setTrue(Mathf.Clamp(falseValue, LESSER_CAP, greaterCap));
    }

    public void setGreaterCap(int greaterCap) {
        this.greaterCap = Mathf.Max(greaterCap, LESSER_CAP);
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
