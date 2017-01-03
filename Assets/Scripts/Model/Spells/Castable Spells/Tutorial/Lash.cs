using Scripts.Model.Characters;
using Scripts.Model.Stats;
using Scripts.Model.Stats.Attributes;
using Scripts.Model.Stats.Resources;
using Scripts.View.Effects;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Spells.Named {

    public class Lash : SpellFactory {
        public const string NAME = "Lash";
        public static readonly string DESCRIPTION = string.Format("Damage a single enemy.");
        public const SpellType SPELL_TYPE = SpellType.OFFENSE;
        public const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
        public const string SUCCESS_TEXT = "{0} lashes {1}!\n{1} took <color=red>{2}</color> damage!";
        public const string MISS_TEXT = "{0} lashes {1}... But it missed!";

        private const float VARIANCE = 0.25f;
        private const int MIN_DAMAGE = 1;
        private const int DEX_INCREASE = 1;

        public Lash() : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE) {
        }

        private const string SOUND_LOCATION = "Whip";

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t, o) => {
                    return Util.Chance(.8);
                },
                calculation: (c, t, o) => {
                    return new Calculation(
                        targetResources: new Dictionary<ResourceType, PairedValue>() {
                        { ResourceType.HEALTH,
                            new PairedValue(0, -Mathf.Max(MIN_DAMAGE, Util.Random(c.GetAttributeCount(AttributeType.STRENGTH, false), VARIANCE)))
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
                    DamageSplat(t, calc.TargetResources[ResourceType.HEALTH].False)
                    };
                }
            );
        }

        public override Miss CreateMiss() {
            return new Miss(
                    createText: (c, t, calc, o) => {
                        return string.Format(MISS_TEXT, c.Name, t.Name);
                    },
                    sfx: (c, t, calc, o) => {
                        return new CharacterEffect[] {
                        new HitsplatEffect(t.Presenter.PortraitView, Color.grey, "MISS"),
                        };
                    }
                    );
        }

        private const float BASE_SHAKE = 3;

        private CharacterEffect ShakeBasedOnDamage(Character target, float damage) {
            float shakePower = BASE_SHAKE * -damage / target.GetResourceCount(ResourceType.HEALTH, true);
            return new ShakeEffect(target.Presenter.PortraitView, shakePower, 0);
        }

        private HitsplatEffect DamageSplat(Character target, float damage, string suffix = "") {
            return new HitsplatEffect(target.Presenter.PortraitView, Color.red, damage + suffix);
        }
    }
}