using System.Collections.Generic;
using Scripts.Model.Spells;
using Scripts.Model.Items;
using Scripts.Model.Stats;
using Scripts.Game.Defined.Spells;
using Scripts.Model.Buffs;
using Scripts.Game.Defined.Serialized.Spells;

namespace Scripts.Game.Defined.Serialized.Items.Consumables {
    public class Apple : ConsumableItem {
        private const int HEALING_AMOUNT = 10;

        public Apple() : base(1, TargetType.SINGLE_ALLY, "Apple", string.Format("A juicy apple. Restores {0} {1}.", HEALING_AMOUNT, StatType.HEALTH.ColoredName)) { }

        public override IList<SpellEffect> GetEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] { new AddToModStat(target.Stats, StatType.HEALTH, HEALING_AMOUNT) };
        }
    }
}

namespace Scripts.Game.Defined.Serialized.Items.Equipment {
    public class PoisonArmor : EquippableItem {
        public PoisonArmor() : base(EquipType.ARMOR, 10, "Poisoned Armor", "This doesn't look safe.") {
            Stats.Add(StatType.VITALITY, 3);
            Stats.Add(StatType.AGILITY, -1);
        }

        public override Buff CreateBuff() {
            return new Poison();
        }

    }

    public class BrokenSword : EquippableItem {
        public BrokenSword() : base(EquipType.WEAPON, 5, "Broken Sword", "A broken sword dropped by a spirit.") {
            Stats.Add(StatType.STRENGTH, 1);
            Stats.Add(StatType.VITALITY, -1);
        }
    }

    public class GhostArmor : EquippableItem {
        public GhostArmor() : base(EquipType.ARMOR, 10, "Cursed Mail", "A cursed chainmail dropped by a spirit.") {
            Stats.Add(StatType.STRENGTH, -10);
            Stats.Add(StatType.AGILITY, -10);
        }
    }
}

namespace Scripts.Game.Defined.Serialized.Items.Misc {
    public class Money : BasicItem {
        public Money()
            : base(
            Util.GetSprite("water-drop"),
            0,
            "Drop",
            "A drop of pure water."
            ) { }
    }
}