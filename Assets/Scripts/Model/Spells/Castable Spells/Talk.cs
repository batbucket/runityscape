using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Talk : SpellFactory {
    public const string NAME = "Talk";
    public static readonly string DESCRIPTION = string.Format("Attempt to talk to a foe.");
    public const SpellType SPELL_TYPE = SpellType.ACT;
    public const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>();

    string text;

    public Talk(string text) : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) {
        this.text = text;
    }

    protected override IDictionary<string, SpellComponent> CreateComponents(Character caster, Character target, Spell spell, SpellDetails other) {
        return new Dictionary<string, SpellComponent>() {
            { SpellFactory.PRIMARY, new SpellComponent(
                hit: new Result(
                    isState: (c, t, o) => {
                        return true;
                    },
                    createText: (c, t, calc, o) => {
                        return string.Format(text, c.Name, t.Name);
                    }
                    )

                )
            }
        };
    }
}
