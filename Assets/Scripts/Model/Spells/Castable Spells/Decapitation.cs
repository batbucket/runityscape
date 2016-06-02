using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Decapitation : SpellFactory {
    const string NAME = "Hero Strike";
    const string DESCRIPTION = "Rush at an enemy and slash at their neck with a <color=yellow>weapon</color>.";
    const SpellType SPELL_TYPE = SpellType.OFFENSE;
    const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
    static readonly IDictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>() {
        { ResourceType.SKILL, 2 }
    };

    public Decapitation() : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) { }

    public override bool IsCastable(Character caster, Character target = null) {
        return base.IsCastable(caster, target) && (caster.Selections[Selection.EQUIP] as Equipment).ContainsEquipment(EquipmentType.WEAPON);
    }

    protected override IDictionary<string, SpellComponent> CreateComponents(Character caster, Character target, Spell spell, SpellDetails other = null) {
        return new Dictionary<string, SpellComponent>() {
            { SpellFactory.PRIMARY,
                new SpellComponent(
                    hit: new Result(
                        isState: (c, t, o) => {
                            return true;
                        },
                        calculation: (c, t, o) => {
                            return new Calculation(
                                targetResources: new Dictionary<ResourceType, PairedInt>() {
                                    { ResourceType.HEALTH,
                                        new PairedInt(0,
                                            -UnityEngine.Random.Range(
                                                Mathf.Min(c.GetAttributeCount(AttributeType.INTELLIGENCE, false), c.GetAttributeCount(AttributeType.STRENGTH, false)) * 2.5f,
                                                Mathf.Max(c.GetAttributeCount(AttributeType.INTELLIGENCE, false), c.GetAttributeCount(AttributeType.STRENGTH, false)) * 2.5f
                                            )
                                        )
                                    }
                                }
                            );
                        },
                        createText: (c, t, calc, o) => {
                            return string.Format("{0} rushes at {1} and slashes at their neck for {2} damage!", c.Name, t.Name, (int)-calc.TargetResources[ResourceType.HEALTH].False);
                        },
                        sfx: (c, t, calc, o) => {
                            Game.Instance.Sound.Play("Sounds/Slash_0");
                            Game.Instance.Effect.CreateBloodsplat(t);
                        }
                    )
                )
            },
        };
    }
}