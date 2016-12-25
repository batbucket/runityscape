using UnityEngine;
using System.Collections;
using System;

public struct Encounter {
    public Func<Page> page;
    public Func<float> weight;
    public Func<bool> overrideCondition;

    public Encounter(Func<Page> page, Func<float> weight, Func<bool> overrideCondition = null) {
        this.page = page;
        this.weight = weight;
        this.overrideCondition = overrideCondition ?? (() => { return false; });
    }
}
