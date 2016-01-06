using UnityEngine;
using System.Collections;
using System;

public abstract class ConsumableItem : Item {
    protected bool consumed;

    public ConsumableItem(Character caster, string name) : base(caster, name) {
        this.consumed = false;
    }

    public override void onFailure(Game game) {
        throw new NotImplementedException();
    }

    public override void onSuccess(Game game) {
        consumed = true;
    }

    public override void calculateSuccessRate() {
        successRate = consumed ? 0 : 1;
    }

    public override void undo(Game game) {
        consumed = false;
    }
}
