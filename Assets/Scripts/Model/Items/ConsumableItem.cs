using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class ConsumableItem : Item {

    public ConsumableItem(string name, string description, int count) : base(name, description, count) { }

    protected override IDictionary<string, SpellComponent> CreateComponents(Character caster, Character target, Spell spell, SpellDetails other) {
        return new Dictionary<string, SpellComponent>() {
            { SpellFactory.PRIMARY, new SpellComponent(
                hit: new Result(
                    isState: (c, t, o) => {
                        return true;
                    },
                    calculation: (c, t, o) => {
                        return GetCalculation(c, t);
                    },
                    perform: (c, t, calc, o) => {
                        caster.Selections[Selection.ITEM].Remove(this);
                        Result.NumericPerform(c, t, calc);
                    },
                    createText: (c, t, calc, o) => {
                        return (c == t) ? SelfUseText(c, t, calc) : OtherUseText(c, t, calc);
                    },
                    sfx: (c, t, calc, o) => {
                        SFX(c, t, calc);
                    }
                )
            )}
        };
    }

    protected abstract Calculation GetCalculation(Character caster, Character target);

    protected abstract string SelfUseText(Character caster, Character target, Calculation calculation);
    protected abstract string OtherUseText(Character caster, Character target, Calculation calculation);

    protected virtual void SFX(Character caster, Character target, Calculation calculation) {
        Game.Instance.Sound.Play("Sounds/Zip_0");
    }
}
