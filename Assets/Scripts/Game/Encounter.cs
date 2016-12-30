using UnityEngine;
using System.Collections;
using System;

public class Encounter {
    public Func<Page> page;
    public Func<float> weight;

    public Encounter(Func<Page> page, Func<float> weight) {
        this.page = page;
        this.weight = weight;
    }
}