using Scripts.Model.Characters;
using Scripts.Model.Stats;
using Scripts.Model.Stats.Attributes;
using Scripts.Model.Stats.Resources;
using Scripts.View.Effects;
using Scripts.View.Effects.Scripts.Model;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Spells.Named {

    public class Attack : SpellFactory {
        public const string CRITICAL_TEXT = "{0} critically strikes {1}!\n{1} took <color=red>{2}</color> damage!";
        public const string MISS_TEXT = "{0} attacks {1}... But it missed!";
        public const string NAME = "Attack";
        public const int SP_GAIN = 1;
        public const SpellType SPELL_TYPE = SpellType.OFFENSE;
        public const string SUCCESS_TEXT = "{0} attacks {1}!\n{1} took <color=red>{2}</color> damage!";
        public const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
        public static readonly string DESCRIPTION = string.Format("Damage a single enemy.");
        private const float CRIT_RATIO = 2f;
        private const int MIN_DAMAGE = 1;
        private const string SOUND_LOCATION = "Slash_0";
        private const float VARIANCE = 0.25f;

        public Attack() : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE) {
        }

        public override Critical CreateCritical() {
            return new Critical(
                    isState: (c, t, o) => {
                        return Util.Chance(.2);
                    },
                    calculation: (c, t, o) => {
                        return new Calculation(targetResources: new Dictionary<ResourceType, PairedValue>() {
                        { ResourceType.HEALTH,
                            new PairedValue(0, -Util.Random(c.GetAttributeCount(AttributeType.STRENGTH, false) * CRIT_RATIO, VARIANCE))
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
                        return Result.AppendToStandard(c, t, calc, new BloodsplatEffect(t.Presenter.PortraitView));
                    }
            );
        }

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t, o) => {
                    return t.State == CharacterState.DEFEAT || Util.Chance(.8);
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

        protected override void OnOnce(Character caster, Spell other) {
            if (caster.HasResource(ResourceType.SKILL)) {
                caster.AddToResource(ResourceType.SKILL, false, SP_GAIN, true);
                caster.Presenter.PortraitView.AddEffect(new HitsplatEffect(caster.Presenter.PortraitView, Color.yellow, "+1"));
            }
        }
    }
}