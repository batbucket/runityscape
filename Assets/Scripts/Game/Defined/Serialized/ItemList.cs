using System.Collections.Generic;
using Scripts.Model.Spells;
using Scripts.Model.Items;
using Scripts.Model.Stats;
using Scripts.Game.Defined.Spells;
using Scripts.Model.Buffs;
using Scripts.Game.Defined.Serialized.Spells;

namespace Scripts.Game.Defined.Serialized.Items.Consumables {
    public class Apple : ConsumableItem {
        private const int HEALING_AMOUNT = 5;

        public Apple() : base(5, TargetType.ALL, "Apple", string.Format("A juicy apple. Restores {0} {1}.", HEALING_AMOUNT, StatType.HEALTH.Name)) { }

        public override IList<SpellEffect> GetEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] { new AddToModStat(caster.Stats, StatType.HEALTH, HEALING_AMOUNT) };
        }
    }
}

namespace Scripts.Game.Defined.Serialized.Items.Equipment {
    public class PoisonArmor : EquippableItem {
        public PoisonArmor() : base(EquipType.ARMOR, 10, "Poisoned Armor", "This doesn't look safe.") {
            Stats.Add(StatType.VITALITY, 3);
            Stats.Add(StatType.AGILITY, -1);
        }

        public override Buff CreateBuff(SpellParams target) {
            return new Poison(target, target);
        }

    }
}

namespace Scripts.Game.Defined.Serialized.Items.Misc {
    public class Money : BasicItem {
        public Money() : base(Util.GetSprite("fox-head"), 0, int.MaxValue, "Drop", "Drop of water, typically used as currency. Drinking it is not advised.") { }
    }
}