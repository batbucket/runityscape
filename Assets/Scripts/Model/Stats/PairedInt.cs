using UnityEngine;
using System.Collections;

/**
 * This class represents a pair of integer values
 */
public abstract class PairedInt {
    int _false;
    int _true;

    public virtual int False { get { return _false; } set { _false = value; } }
    public virtual int True { get { return _true; } set { _true = value; } }

    public bool IsMaxed() {
        return False <= True;
    }

    public void Set(int both) {
        True = both;
        False = both;
    }

    public void Add(int both) {
        True += both;
        False += both;
    }

    public void Subtract(int both) {
        True -= both;
        False -= both;
    }

    public void SetPercentage(float percentage) {
        SetTruePercentage(percentage);
        SetFalsePercentage(percentage);
    }

    public void SetTruePercentage(float percentage) {
        True = (int)(True * percentage);
    }

    public void SetFalsePercentage(float percentage) {
        False = (int)(False * percentage);
    }

    public float GetRatio() {
        return ((float)True) / False;
    }

    /**
     * Reset falseValue to be trueValue
     */
    public void Reset() {
        False = True;
    }

    /**
     * resetValue = trueValue * percentage
     * set falseValue to resetValue if
     * it's greater, otherwise, keep falseValue
     */
    public void Reset(float percentage) {
        int resetValue = (int)(percentage * False);
        False = Mathf.Max(resetValue, True);
    }
}
