using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Vulnerablize : SpellFactory {
    const SpellType SPELL_TYPE = SpellType.MERCY;
    const TargetType TARGET_TYPE = TargetType.SELF;

    public Vulnerablize() : base(null, null, SPELL_TYPE, TARGET_TYPE) { }

    protected override IDictionary<string, SpellComponent> CreateComponents(Character caster, Character target, Spell spell, SpellDetails other) {
        return new Dictionary<string, SpellComponent>() {
            { SpellFactory.PRIMARY, new SpellComponent(
                hit: new Result(
                    isState: (c, t, o) => {
                        return true;
                    },
                    perform: (c, t, calc, o) => {
                        spell.CurrentString = "SpareKill";
                    }
                ))
            },
            { "SpareKill", new CounterSpellComponent(sprite: Util.GetSprite("Icons/cracked-shield"), isGood: false, counterSpellFactory: new Vulnerable(), count: 1, isReact: (s, r, c) => s.SpellFactory.SpellType == SpellType.OFFENSE) }
        };
    }
}
