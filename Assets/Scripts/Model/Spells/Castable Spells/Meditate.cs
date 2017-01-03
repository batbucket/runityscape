using Scripts.Model.Stats;
using Scripts.Model.Stats.Resources;
using System.Collections.Generic;

namespace Scripts.Model.Spells.Named {

    public class Meditate : SpellFactory {
        public const string DESCRIPTION = "Heal yourself for 50% of your maximum <color=lime>life</color>.";
        public const string NAME = "Meditate";
        public const SpellType SPELL_TYPE = SpellType.BOOST;
        public const TargetType TARGET_TYPE = TargetType.SELF;
        public static readonly string CAST_TEXT = "{0} calms their mind!\n{0} restored <color=lime>{1}</color> life!";

        public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>() {
        {ResourceType.SKILL, 3 }
    };

        public Meditate() : base(SPELL_TYPE, TARGET_TYPE, NAME, DESCRIPTION, new Cost(ResourceType.SKILL, 3)) {
        }

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t, o) => {
                    return true;
                },
                calculation: (c, t, o) => {
                    return new Calculation(
                        targetResources: new Dictionary<ResourceType, PairedValue>() {
                    { ResourceType.HEALTH, new PairedValue(0, t.GetResourceCount(ResourceType.HEALTH, true) / 2) }
                        });
                },
                createText: (c, t, calc, o) => {
                    return string.Format(CAST_TEXT, t.DisplayName, calc.TargetResources[ResourceType.HEALTH].False);
                },
                sound: (c, t, calc, o) => {
                    return "Zip_0";
                }
            );
        }
    }
}