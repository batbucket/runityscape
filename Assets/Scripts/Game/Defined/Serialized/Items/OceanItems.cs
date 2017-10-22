using System;
using System.Collections.Generic;
using Scripts.Game.Defined.Serialized.Buffs;
using Scripts.Model.Buffs;
using Scripts.Model.Characters;
using Scripts.Model.Items;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using Scripts.Game.Defined.Spells;
using Scripts.Game.Defined.Unserialized.Items;

namespace Scripts.Game.Defined.Serialized.Items {

    public class FishHook : EquippableItem {

        public FishHook() : base(EquipType.WEAPON, 50, "Fish Hook", "A used fish hook.") {
            AddFlatStatBonus(StatType.STRENGTH, 5);
            AddFlatStatBonus(StatType.AGILITY, 1);
            AddFlatStatBonus(StatType.VITALITY, -1);
        }

        public override PermanentBuff CreateBuff() {
            return new FishShook();
        }
    }

    public class SharkFin : ConsumableItem {
        private const int HEALING_AMOUNT = 25;

        public SharkFin() : base(75, TargetType.ONE_ALLY, "Shark fin", string.Format("A delicious. Restores {0} {1}.", HEALING_AMOUNT, StatType.HEALTH.ColoredName)) {
        }

        public override IList<SpellEffect> GetEffects(Character caster, Character target) {
            return new SpellEffect[] { new AddToModStat(target.Stats, StatType.HEALTH, HEALING_AMOUNT) };
        }
    }

    public class Cleansing : ConsumableItem {
        private const int DAMAGE = 5;

        public Cleansing() : base(20, TargetType.ONE_ALLY, "Cleanse", string.Format("Caster takes {0} damage. Removes all dispellable buffs from a target.", DAMAGE)) {
        }

        public override IList<SpellEffect> GetEffects(Character caster, Character target) {
            return new SpellEffect[] {
                new DispelAllBuffs(target.Buffs),
                new AddToModStat(caster.Stats, StatType.HEALTH, -DAMAGE)
            };
        }
    }

    public class VitalityTrinket : SingleStatTrinket {
        public VitalityTrinket() : base(StatType.VITALITY, 5, "vitality", 50) {
        }
    }

    public class AgilityTrinket : SingleStatTrinket {
        public AgilityTrinket() : base(StatType.AGILITY, 5, "agility", 50) {
        }
    }

    public class IntellectTrinket : SingleStatTrinket {
        public IntellectTrinket() : base(StatType.INTELLECT, 5, "intellect", 50) {
        }
    }

    public class StrengthTrinket : SingleStatTrinket {
        public StrengthTrinket() : base(StatType.STRENGTH, 5, "strength", 50) {
        }
    }
}