using Scripts.Model.Stats;
using Scripts.Model.Stats.Attributes;
using Scripts.Model.Stats.Resources;
using Scripts.View.Effects;
using System.Collections.Generic;

namespace Scripts.Model.Spells.Named {

    public class Smite : SpellFactory {
        public const int DAMAGE = 9;
        public static readonly string DESCRIPTION = string.Format("Deal <color=red>{0}</color>+<color=blue>INT</color> unresistable damage from afar to a single enemy.", DAMAGE);
        private const string NAME = "Smite";
        private const SpellType SPELL_TYPE = SpellType.OFFENSE;
        private const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
        private static readonly string TEXT = "{0} invokes the wrath of heaven on {1}!\n{1} takes <color=red>{2}</color> damage!";

        public Smite() : base(SPELL_TYPE, TARGET_TYPE, NAME, DESCRIPTION, new Cost(ResourceType.SKILL, 2)) {
        }

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t) => true,
                calculation: (c, t) => {
                    return new Calculation(
                       targetResources: new Dictionary<ResourceType, PairedValue>() {
                       { ResourceType.HEALTH, new PairedValue(0, -DAMAGE - c.GetAttributeCount(AttributeType.INTELLIGENCE, false)) }
                        }
                    );
                },
                createText: (c, t, calc) => {
                    return string.Format(TEXT, c.DisplayName, t.DisplayName, (int)-calc.TargetResources[ResourceType.HEALTH].False);
                },
                sound: (c, t, calc) => {
                    return "Boom_4";
                },
                sfx: (c, t, calc) => {
                    return Result.AppendToStandard(c, t, calc, new LightningEffect(t.Presenter.PortraitView));
                }
            );
        }
    }
}