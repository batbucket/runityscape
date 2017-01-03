using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using Scripts.Model.Stats.Resources;
using System.Collections.Generic;

namespace Scripts.Model.Items.Named {

    public class Lobster : ConsumableItem {
        public const string NAME = "Lobster";
        public static readonly string DESCRIPTION = string.Format("Eat a Lobster for {0} HP, just like in RuneScape™. Property of Jagex Ltd. The most popular Free-To-Play MMORPG played by millions worldwide.", HEAL_AMOUNT);
        public const int HEAL_AMOUNT = -112;
        public const string USE_TEXT_SELF = "* {1} ate a Lobster, restoring {2} life!";
        public const string USE_TEXT_OTHER = "* {0} fed a Lobster to {1}, restoring {2} life!";

        public Lobster() : base(NAME, DESCRIPTION) {
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