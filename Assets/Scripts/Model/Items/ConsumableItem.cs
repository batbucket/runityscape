using UnityEngine;
using System.Collections;
using System;

public abstract class ConsumableItem : Item {
    protected bool consumed;

    public ConsumableItem(Character caster, string name) : base(caster, name) {
        this.consumed = false;
    }

    public override void onFailure() {
        throw new NotImplementedException();
    }

    public override void onSuccess() {
        consumed = true;
    }

    public override bool canCast() {
        return !consumed;
    }

    public override void undo() {
        consumed = false;
    }
}
