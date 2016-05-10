using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Counter : SpellFactory {
    static readonly string name = "Counter";
    static readonly string description = "Strike back on your attacker.";
    static readonly SpellType spellType = SpellType.DEFENSE;
    static readonly TargetType targetType = TargetType.SELF;
    static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>();

    public Counter() : base(name, description, spellType, targetType, COSTS) {
    }

    protected override IDictionary<string, SpellComponent> CreateComponents(Character caster, Character target, Spell spell, SpellDetails other) {
        return new Dictionary<string, SpellComponent>() {
            { SpellFactory.PRIMARY, new SpellComponent(
                hit: new Result(
                    isState: (c, t, o) => {
                        return true;
                    },
                    perform: (s, r, c, o) => {
                        spell.CurrentString = "COUNTER";
                    },
                    createText: (s, r, c, o) => {
                        return string.Format("* {0} readies themselves for a counterattack!", caster.Name);
                    }
                ))
            },
            { "COUNTER", new CounterSpellComponent(
                sprite: Util.GetSprite("Icons/Icon.1_15"),
                isGood: true,
                count: 1,
                counterSpellFactory: new CounterAttack(),
                isReact: (s, r, c) => {
                    return s.SpellFactory.SpellType == SpellType.OFFENSE && s.Caster != caster;
                },
                hit: new Result(
                    isState: (c, t, o) => {
                        return true;
                    },
                    calculation: (c, t, o) => {
                        return new Calculation(
                            targetResources:
                                new Dictionary<ResourceType, PairedInt>() {
                                    { ResourceType.HEALTH, new PairedInt(0, UnityEngine.Random.Range(c.GetAttributeCount(AttributeType.INTELLIGENCE, false), c.GetAttributeCount(AttributeType.STRENGTH, false))) }
                                });
                    },
                    createText: (c, t, calc, o) => {
                        return string.Format("{0} strikes back at {1} for {2} damage!", c.Name, t.Name, calc.TargetResources[ResourceType.HEALTH].False);
                    }
                ),
                critical: null,
                miss: null,
                totalDuration: 10)
            }
        };
    }
}
