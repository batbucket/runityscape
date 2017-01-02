using UnityEngine;
using System.Collections;

/**
 * This class represents an integer value paired with a float
 */
public class PairedInt {
    float _false;
    int _true;

    public virtual float False { get { return _false; } set { _false = value; } }
    public virtual int True { get { return _true; } set { _true = value; } }

    public PairedInt(int initial) : this(initial, initial) { }

    public PairedInt(int trueValue, int falseValue) {
        _true = trueValue;
        _false = falseValue;
    }
}