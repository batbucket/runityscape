using UnityEngine;
using System.Collections;
using System;

public abstract class ConsumableItem : Item {

    public ConsumableItem(Character caster, string name, string description, int count) : base(caster, name, description, count) {
    }

    public override void onFailure() {
        throw new NotImplementedException();
    }
}
