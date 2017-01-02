using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class ConsumableItem : Item {

    public ConsumableItem(string name, string description) : base(name, description) { }

    protected abstract string SelfUseText(Character caster, Character target, Calculation calculation);
    protected abstract string OtherUseText(Character caster, Character target, Calculation calculation);

    protected virtual void SFX(Character caster, Character target, Calculation calculation) {
        Game.Instance.Sound.Play("Sounds/Zip_0");
    }

    protected abstract Calculation CreateCalculation(Character caster, Character target);

    protected virtual void OnPerform(Character caster, Character target) { }


    public override Hit CreateHit() {
        return new Hit(
            isState: (c, t, o) => true,
            calculation: (c, t, o) => {
                return CreateCalculation(c, t);
            },
            perform: (c, t, calc, o) => {
                Result.NumericPerform(c, t, calc);
                c.Inventory.Remove(this);
                OnPerform(c, t);
            },
            createText: (c, t, calc, o) => {
                if (c == t) {
                    return SelfUseText(c, t, calc);
                } else {
                    return OtherUseText(c, t, calc);
                }
            }
        );
    }

    public override Critical CreateCritical() {
        return base.CreateCritical();
    }

    public override Miss CreateMiss() {
        return base.CreateMiss();
    }
}
