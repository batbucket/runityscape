using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using Scripts.Model.Stats.Resources;
using System.Collections.Generic;

namespace Scripts.Model.Items.Named {

    public class LifePotion : ConsumableItem {
        public const string NAME = "LifePotion";
        public static readonly string DESCRIPTION = string.Format("Heal an ally for <color=lime>{0}</color> Life.", HEAL_AMOUNT);
        public const int HEAL_AMOUNT = 20;
        public const string USE_TEXT_SELF = "{1} uses a life potion!\n{1} restored <color=lime>{2}</color> life!";
        public const string USE_TEXT_OTHER = "{0} uses a life potion on {1}!\n{1} restored <color=lime>{2}</color> life!";

        public LifePotion() : base(NAME, DESCRIPTION) {
        }

        protected override Calculation CreateCalculation(Character caster, Character target) {
            return new Calculation(targetResources: new Dictionary<ResourceType, PairedValue>() { { ResourceType.HEALTH, new PairedValue(0, HEAL_AMOUNT) } });
        }

        protected override string SelfUseText(Character caster, Character target, Calculation calculation) {
            return string.Format(USE_TEXT_SELF, caster.DisplayName, target.DisplayName, calculation.TargetResources[ResourceType.HEALTH].False);
        }

        protected override string OtherUseText(Character caster, Character target, Calculation calculation) {
            return string.Format(USE_TEXT_OTHER, caster.DisplayName, target.DisplayName, calculation.TargetResources[ResourceType.HEALTH].False);
        }
    }
}