using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Attack : SpellFactory {
    public const string NAME = "Attack";
    public static readonly string DESCRIPTION = string.Format("Attack a single enemy for {0} damage.", Util.Color(AttributeType.STRENGTH.ShortName, AttributeType.STRENGTH.Color));
    public const SpellType SPELL_TYPE = SpellType.OFFENSE;
    public const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
    public const string SUCCESS_TEXT = "* {0} attacks {1} for {2} damage!";
    public const string CRITICAL_TEXT = "* {0} critically strikes {1} for {2} damage!";
    public const string MISS_TEXT = "* {0} attacks {1}... But it missed!";
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>();
    public const int SP_GAIN = 1;

    [SerializeField]
    Sprite bleedIcon;

    public Attack() : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) { }



    protected override void OnOnce(Character caster) {
        caster.AddToResource(ResourceType.SKILL, false, 1, true);
    }

    protected override IDictionary<string, SpellComponent> CreateComponents(Character caster, Character target, Spell spell) {
        return new Dictionary<string, SpellComponent>() {
            { SpellFactory.PRIMARY, new SpellComponent(
                hit: new Result(
                    isState: (c, t) => {
                        return Util.Chance(.8);
                    },
                    calculation: (c, t) => {
                        return new Calculation(
                            targetResources: new Dictionary<ResourceType, PairedInt>() {
                                { ResourceType.HEALTH, new PairedInt(0, -UnityEngine.Random.Range(c.GetAttributeCount(AttributeType.INTELLIGENCE, false), t.GetAttributeCount(AttributeType.STRENGTH, false))) }
                            }
                        );
                    },
                    perform: (c, t, calc) => {
                        Result.NumericPerform(c, t, calc);
                        spell.CurrentString = "Bleed";
                    },
                    createText: (c, t, calc) => {
                        return string.Format(SUCCESS_TEXT, c.Name, t.Name, -calc.TargetResources[ResourceType.HEALTH].False);
                    },
                    sfx: (c, t, calc) => {
                        Game.Instance.Sound.Play("Sounds/Attack_0");
                    }
                ),
                critical: new Result(
                        isState: (c, t) => {
                            return Util.Chance(0); //Todo change this to .2
                        },
                        calculation: (c, t) => {
                            return new Calculation(targetResources: new Dictionary<ResourceType, PairedInt>() {
                                { ResourceType.HEALTH, new PairedInt(0, -UnityEngine.Random.Range(c.GetAttributeCount(AttributeType.INTELLIGENCE, false) * 2, t.GetAttributeCount(AttributeType.STRENGTH, false) * 2)) }
                            }
                            );
                        },
                        createText: (c, t, calc) => {
                            return string.Format(CRITICAL_TEXT, c.Name, t.Name, -calc.TargetResources[ResourceType.HEALTH].False);
                        },
                        sfx: (c, t, calc) => {
                            Game.Instance.Sound.Play("Sounds/Attack_0");
                            Game.Instance.Effect.CreateBloodsplat(t);
                        }
                ),
                miss: new Result(
                    isState: (c, t) => {
                        return true;
                    },
                    calculation: (c, t) => {
                        return new Calculation(
                            targetResources: new Dictionary<ResourceType, PairedInt>() {
                                { ResourceType.HEALTH, new PairedInt(0, -UnityEngine.Random.Range(c.GetAttributeCount(AttributeType.INTELLIGENCE, false) * 2, t.GetAttributeCount(AttributeType.STRENGTH, false) * 2)) }
                            }
                        );
                    },
                    perform: (c, t, calc) => {
                        //it's literally nothing
                    },
                    createText: (c, t, calc) => {
                        return string.Format(MISS_TEXT, c.Name, t.Name);
                    },
                    sfx: (c, t, calc) => {
                        Game.Instance.Sound.Play("Sounds/Attack_0");
                        Game.Instance.Effect.CreateBloodsplat(t);
                    })
            )
            },

            { "Bleed", new TimedSpellComponent(
                sprite: Util.LoadIcon("Icon.1_06"),
                isGood: false,
                hit: new Result(
                    isState: (c, t) => {
                        return true;
                    },
                    calculation: (c, t) => {
                        return new Calculation(
                            targetResources: new Dictionary<ResourceType, PairedInt>() {
                                { ResourceType.HEALTH, new PairedInt(0, -UnityEngine.Random.Range(c.GetAttributeCount(AttributeType.INTELLIGENCE, false), t.GetAttributeCount(AttributeType.STRENGTH, false))) }
                            }
                        );
                    },
                    //Implicit perform that just does Character + Calculation values is created here
                    createText: (c, t, calc) => {
                        return string.Format("* {1} bled for {2} damage!", c.Name, t.Name, -calc.TargetResources[ResourceType.HEALTH].False);
                    },
                    sfx: (c, t, calc) => {
                        Game.Instance.Sound.Play("Sounds/Attack_0");
                    }
                ),
                critical: null,
                miss: null,
                timePerTick: .5f,
                totalDuration: 5.0f,
                onEnd: () => { spell.IsFinished = true; }
            )
            }
        };
    }
}
