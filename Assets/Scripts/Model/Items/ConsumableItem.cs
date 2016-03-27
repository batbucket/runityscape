using UnityEngine;
using System.Collections;
using System;

public abstract class ConsumableItem : Item {
    public ConsumableItem(string name, string description, int count) : base(name, description, count) { }
}
