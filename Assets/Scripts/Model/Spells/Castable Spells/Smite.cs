using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Smite : SpellFactory {
    const string NAME = "Smite";
    static readonly string DESCRIPTION = string.Format("Consume all {0}. Deal <color=red>{1}</color> damage per {0} consumed.", Util.Color(ResourceType.SKILL.Name, ResourceType.SKILL.FillColor), DAMAGE_PER_SKILL);
    const SpellType SPELL_TYPE = SpellType.OFFENSE;
    const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
    static readonly IDictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>() {
        { ResourceType.SKILL, 1 }
    };
    public const int DAMAGE_PER_SKILL = 5;

    public Smite() : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) { }

    protected override IDictionary<string, SpellComponent> CreateComponents(Character caster, Character target, Spell spell, SpellDetails other = null) {
        int casterResourceCount = caster.GetResourceCount(ResourceType.SKILL, false) + 1; //Count the 1 SP we consumed when casting this spell
        return new Dictionary<string, SpellComponent>() {
            { SpellFactory.PRIMARY,
                new SpellComponent(
                    hit: new Result(
                        isState: (c, t, o) => {
                            return true;
                        },
                        calculation: (c, t, o) => {
                            return new Calculation(
                                casterResources: new Dictionary<ResourceType, PairedInt>() {
                                    { ResourceType.SKILL,
                                        new PairedInt(0,
                                            -casterResourceCount
                                        )
                                    }
                                },
                                targetResources: new Dictionary<ResourceType, PairedInt>() {
                                    { ResourceType.HEALTH,
                                        new PairedInt(0,
                                            -casterResourceCount * DAMAGE_PER_SKILL
                                        )
                                    },
                                }
                            );
    },
                        createText: (c, t, calc, o) => {
                            return string.Format("{0} invokes the heavens on {1} for <color=red>{2}</color> damage!", c.Name, t.Name, (int)-calc.TargetResources[ResourceType.HEALTH].False);
                        },
                        sfx: (c, t, calc, o) => {
                            Game.Instance.Sound.Play("Sounds/Boom_4");
                            Game.Instance.Effect.LightningEffect(t);
                        }
                    )
                )
            },
        };
    }
}