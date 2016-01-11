using UnityEngine;
using System.Collections;
using System;

public class EquippableItem : Item {
    public EquippableItem(Character caster, string name, string description, int count) : base(caster, name, description, count) {
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
