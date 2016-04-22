using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class ConsumableItem : Item {

    public ConsumableItem(string name, string description, int count) : base(name, description, count) { }

    protected override void ConsumeResources(Character caster) {
        base.ConsumeResources(caster);
    }

    protected override IDictionary<string, SpellComponent> CreateComponents(Character caster, Character target, Spell spell) {
        return new Dictionary<string, SpellComponent>() {
            { SpellFactory.PRIMARY, new SpellComponent(
                hit: new Result(
                    isState: (c, t) => {
                        return true;
                    },
                    calculation: GetCalculation,
                    perform: (c, t, calc) => {
                        caster.Selections[Selection.ITEM].Remove(this);
                    },
                    createText: (c, t, calc) => {
                        return (c == t) ? SelfUseText(c, t, calc) : OtherUseText(c, t, calc);
                    },
                    sfx: SFX
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
