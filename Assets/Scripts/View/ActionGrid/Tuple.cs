using Scripts.Model.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tuple {
    public readonly int Index;
    public readonly IButtonable Data;

    public Tuple(int index, IButtonable data) {
        this.Index = index;
        this.Data = data;
    }
}
