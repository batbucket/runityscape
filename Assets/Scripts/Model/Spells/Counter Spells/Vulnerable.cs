using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Vulnerable : CounterSpellFactory {
    public const SpellType SPELL_TYPE = SpellType.MERCY;
    public const TargetType TARGET_TYPE = TargetType.SELF;
    public const string SUCCESS_TEXT = "{0} was mortally wounded by {1}.";

    public Vulnerable() : base(SPELL_TYPE, TARGET_TYPE) { }

    protected override void OnOnce(Character caster, SpellDetails other) {
        other.Calculation.TargetResources[ResourceType.HEALTH].False = -9999999;
    }

    protected override IDictionary<string, SpellComponent> CreateComponents(Character caster, Character target, Spell spell, SpellDetails other) {
        return new Dictionary<string, SpellComponent>() {
            { SpellFactory.PRIMARY, new SpellComponent(
                hit: new Result(
                    isState: (c, t, o) => {
                        return true;
                    }
                ))
            }
        };
    }
}
