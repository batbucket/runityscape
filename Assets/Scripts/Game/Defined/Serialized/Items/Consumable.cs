using System;
using System.Collections.Generic;
using Scripts.Model.Items;
using Scripts.Model.Spells;
using Scripts.Game.Defined.Spells;
using Scripts.Model.Stats;
using Scripts.Model.Characters;

namespace Scripts.Game.Defined.Serialized.Items {

    public class Apple : ConsumableItem {
        private const int HEALING_AMOUNT = 10;

        public Apple() : base(1, TargetType.SINGLE_ALLY, "Apple", string.Format("A juicy apple. Restores {0} {1}.", HEALING_AMOUNT, StatType.HEALTH.ColoredName)) {
        }

        public override IList<SpellEffect> GetEffects(Character caster, Character target) {
            return new SpellEffect[] { new AddToModStat(target.Stats, StatType.HEALTH, HEALING_AMOUNT) };
        }
    }
}