using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Attack : SpellFactory {
    public const string NAME = "Attack";
    public static readonly string DESCRIPTION = string.Format("Damage a single enemy.");
    public const SpellType SPELL_TYPE = SpellType.OFFENSE;
    public const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
    public const string SUCCESS_TEXT = "{0} attacks {1} for <color=red>{2}</color> damage!";
    public const string CRITICAL_TEXT = "{0} critically strikes {1} for <color=red>{2}</color> damage!";
    public const string MISS_TEXT = "{0} attacks {1}... But it missed!";
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>();
    public const int SP_GAIN = 1;

    const float VARIANCE = 0.25f;
    const float CRIT_RATIO = 2f;
    const int MIN_DAMAGE = 1;

    [SerializeField]
    Sprite bleedIcon;

    public Attack() : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) {
    }

    protected override void OnOnce(Character caster, SpellDetails other) {
        caster.AddToResource(ResourceType.SKILL, false, 1, true);
    }

    private const string SOUND_LOCATION = "Slash_0";

    public override Hit CreateHit() {
        return new Hit(
            isState: (c, t, o) => {
                return Util.Chance(.8);
            },
            calculation: (c, t, o) => {
                return new Calculation(
                    targetResources: new Dictionary<ResourceType, PairedInt>() {
                        { ResourceType.HEALTH,
                            new PairedInt(0, -Mathf.Max(MIN_DAMAGE, Util.Random(c.GetAttributeCount(AttributeType.STRENGTH, false), VARIANCE)))
                        }
                    }
                );
            },
            createText: (c, t, calc, o) => {
                return string.Format(SUCCESS_TEXT, c.DisplayName, t.DisplayName, -calc.TargetResources[ResourceType.HEALTH].False);
            },
            sound: (c, t, calc, o) => {
                return SOUND_LOCATION;
            },
            sfx: (c, t, calc, o) => {
                return new CharacterEffect[] {
                    ShakeBasedOnDamage(t, calc.TargetResources[ResourceType.HEALTH].False),
                    DamageSplat(t, calc.TargetResources[ResourceType.HEALTH].False),
                };
            }
        );
    }

    public override Critical CreateCritical() {
        return new Critical(
                isState: (c, t, o) => {
                    return Util.Chance(.2);
                },
                calculation: (c, t, o) => {
                    return new Calculation(targetResources: new Dictionary<ResourceType, PairedInt>() {
                        { ResourceType.HEALTH,
                            new PairedInt(0, -Util.Random(c.GetAttributeCount(AttributeType.STRENGTH, false) * CRIT_RATIO, VARIANCE))
                        }
                    }
                    );
                },
                createText: (c, t, calc, o) => {
                    return string.Format(CRITICAL_TEXT, c.DisplayName, t.DisplayName, -calc.TargetResources[ResourceType.HEALTH].False);
                },
                sound: (c, t, calc, o) => {
                    return SOUND_LOCATION;
                },
                sfx: (c, t, calc, o) => {
                    return new CharacterEffect[] {
                        DamageSplat(t, calc.TargetResources[ResourceType.HEALTH].False, "!"),
                        ShakeBasedOnDamage(t, calc.TargetResources[ResourceType.HEALTH].False),
                        new BloodsplatEffect(t.Presenter.PortraitView),
                    };
                }
        );
    }

    public override Miss CreateMiss() {
        return base.CreateMiss();
    }

    private const float BASE_SHAKE = 3;
    private CharacterEffect ShakeBasedOnDamage(Character target, float damage) {
        float shakePower = BASE_SHAKE * -damage / target.GetResourceCount(ResourceType.HEALTH, true);
        Debug.Log(shakePower);
        return new ShakeEffect(target.Presenter.PortraitView, shakePower, 0);
    }

    private HitsplatEffect DamageSplat(Character target, float damage, string suffix = "") {
        return new HitsplatEffect(target.Presenter.PortraitView, Color.red, damage + suffix);
    }
}
