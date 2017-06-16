using System.Collections.Generic;
using Scripts.Model.Spells;
using Scripts.Model.Items;
using Scripts.Model.Stats;
using Scripts.Game.Defined.Spells;
using Scripts.Model.Buffs;

namespace Scripts.Game.Defined.Items.Consumables {
    public class Apple : ConsumableItem {
        private const int HEALING_AMOUNT = 5;

        public Apple() : base(Util.GetSprite("fox-head"), 5, 5, TargetType.ALL, "Apple", string.Format("A juicy apple. Restores {0} {1}.", HEALING_AMOUNT, StatType.HEALTH.Name)) { }

        public override IList<SpellEffect> GetEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] { new AddToModStat(caster.Stats, StatType.HEALTH, HEALING_AMOUNT) };
        }
    }
}

namespace Scripts.Game.Defined.Items.Equipment {
    public class PoisonArmor : EquippableItem {
        public PoisonArmor() : base(Util.GetSprite("fox-head"), EquipType.ARMOR, 10, 3, "Poisoned Armor", "This doesn't look safe.") {
            Stats.Add(StatType.VITALITY, 3);
            Stats.Add(StatType.AGILITY, -1);
        }

        public override Buff CreateBuff(SpellParams target) {
            return new Poison(target, target);
        }

    }
}

namespace Scripts.Game.Defined.Items.Misc {
    public class Money : BasicItem {
        public Money(int count) : base(Util.GetSprite("fox-head"), 0, count, 99999, "Drop", "Drop of water, typically used as currency. Drinking it is not advised.") { }
    }
}