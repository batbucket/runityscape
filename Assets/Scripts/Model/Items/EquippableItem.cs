using UnityEngine;
using System.Collections;
using System;

public class EquippableItem : Item {
    public EquippableItem(Character caster, string name, string description, int count) : base(name, description, count) {
    }

    public override int CalculateAmount(Character caster, Character target) {
        throw new NotImplementedException();
    }

    public override double CalculateHitRate(Character caster, Character target) {
        throw new NotImplementedException();
    }

    protected override void OnFailure(Character caster, Character target) {
        throw new NotImplementedException();
    }

    protected override void OnSuccess(Character caster, Character target) {
        throw new NotImplementedException();
    }

    public override void Undo() {
        throw new NotImplementedException();
    }

    protected override void OnHit(Character caster, Character target) {
        throw new NotImplementedException();
    }

    protected override void OnMiss(Character caster, Character target) {
        throw new NotImplementedException();
    }
}
