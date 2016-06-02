using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Spare : SpellFactory {
    public const string NAME = "Spare";
    public static readonly string DESCRIPTION = string.Format("Attempt to spare a foe.");
    public const SpellType SPELL_TYPE = SpellType.MERCY;
    public const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
    public const string SUCCESS_TEXT = "{0} is sparing {1}.";
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>();

    public Spare() : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) { }

    protected override IDictionary<string, SpellComponent> CreateComponents(Character caster, Character target, Spell spell, SpellDetails other) {
        return new Dictionary<string, SpellComponent>() {
            { SpellFactory.PRIMARY, new SpellComponent(
                hit: new Result(
                    isState: (c, t, o) => {
                        return true;
                    },
                    perform: (c, t, calc, o) => {
                        Result.NumericPerform(c, t, calc);
                        (new Vulnerablize()).Cast(c, c);
                        c.IsCharging = false;
                    },
                    createText: (c, t, calc, o) => {
                        return string.Format(SUCCESS_TEXT, c.Name, t.Name);
                    },
                    sfx: (c, t, calc, o) => {
                        Game.Instance.Sound.Play("Sounds/Blip_0");
                    }
                )
            )
            }
        };
    }
}
