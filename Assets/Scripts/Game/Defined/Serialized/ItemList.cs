using System.Collections.Generic;
using Scripts.Model.Spells;
using Scripts.Model.Items;
using Scripts.Model.Stats;
using Scripts.Game.Defined.Spells;
using Scripts.Model.Buffs;
using Scripts.Game.Defined.Serialized.Spells;

namespace Scripts.Game.Defined.Serialized.Items.Consumables {
    public class Apple : ConsumableItem {
        private const int HEALING_AMOUNT = 3;

        public Apple() : base(5, TargetType.SINGLE_ALLY, "Apple", string.Format("A juicy apple. Restores {0} {1}.", HEALING_AMOUNT, StatType.HEALTH.Name)) { }

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
        public BrokenSword() : base(EquipType.WEAPON, 5, "Broken Sword", "A broken sword dropped by a spirit. You feel closer to death just by holding it.") {
            Stats.Add(StatType.STRENGTH, 1);
            Stats.Add(StatType.VITALITY, -1);
        }
    }

    public class GhostArmor : EquippableItem {
        public GhostArmor() : base(EquipType.ARMOR, 10, "Ghostly Mail", "A transparent, weightless chainmail. You feel closer to death just by holding it.") {
            Stats.Add(StatType.AGILITY, 1);
            Stats.Add(StatType.VITALITY, -1);
        }
    }
}

namespace Scripts.Game.Defined.Serialized.Items.Misc {
    public class Money : BasicItem {
        public Money() : base(Util.GetSprite("fox-head"), 0, "Drop", "Drop of water, typically used as currency. Drinking it is not advised.") { }
    }
}