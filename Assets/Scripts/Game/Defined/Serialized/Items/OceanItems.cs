using System;
using System.Collections.Generic;
using Scripts.Game.Defined.Serialized.Buffs;
using Scripts.Model.Buffs;
using Scripts.Model.Characters;
using Scripts.Model.Items;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using Scripts.Game.Defined.Spells;

namespace Scripts.Game.Defined.Serialized.Items {

    public class FishHook : EquippableItem {

        public FishHook() : base(EquipType.WEAPON, 50, "Fish Hook", "A used fish hook.") {
            AddFlatStatBonus(StatType.STRENGTH, 5);
            AddFlatStatBonus(StatType.AGILITY, 1);
            AddFlatStatBonus(StatType.VITALITY, -1);
        }

        public override Buff CreateBuff() {
            return new FishShook();
        }
    }

    public class Cleanse : ConsumableItem {
        private const int DAMAGE = 5;

        public Cleanse() : base(20, TargetType.ONE_ALLY, "Cleanse", string.Format("Caster takes {0} damage. Removes all dispellable buffs from a target.", DAMAGE)) {
        }

        public override IList<SpellEffect> GetEffects(Character caster, Character target) {
            return new SpellEffect[] {
                new DispelAllBuffs(target.Buffs),
                new AddToModStat(caster.Stats, StatType.HEALTH, -DAMAGE)
            };
        }
    }
}