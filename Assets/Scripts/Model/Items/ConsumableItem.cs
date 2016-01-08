using UnityEngine;
using System.Collections;
using System;

public abstract class ConsumableItem : Item {

    public ConsumableItem(Character caster, string name, int count) : base(caster, name, count) {
    }

    public override void onFailure() {
        throw new NotImplementedException();
    }
}
