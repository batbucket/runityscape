using UnityEngine;
using System.Collections;
using System;

public abstract class ConsumableItem : Item {

    public ConsumableItem(string name, string description, int count) : base(name, description, count) {
    }

    public override void Undo() {
        Item oneItem = (Item)Clone();
        oneItem.Count = 1;
        Caster.Items.Add(oneItem);
    }
}
