using UnityEngine;
using System.Collections;
using System;

public class EquippableItem : Item {
    public EquippableItem(Character caster, string name) : base(caster, name) {
    }

    public override void onFailure() {
        throw new NotImplementedException();
    }

    public override void onSuccess() {
        throw new NotImplementedException();
    }

    public override void undo() {
        throw new NotImplementedException();
    }
}
