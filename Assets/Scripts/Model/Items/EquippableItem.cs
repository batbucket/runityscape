using UnityEngine;
using System.Collections;
using System;

public abstract class EquippableItem : Item {
    public EquippableItem(Character caster, string name, string description, int count) : base(name, description, count) {
    }
}
