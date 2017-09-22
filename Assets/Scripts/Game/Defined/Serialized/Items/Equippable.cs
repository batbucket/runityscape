using Scripts.Game.Defined.Serialized.Buffs;
using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Model.Buffs;
using Scripts.Model.Items;
using Scripts.Model.Stats;

namespace Scripts.Game.Defined.Serialized.Items {

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
            Stats.Add(StatType.VITALITY, -4);
        }
    }

    public class GhostArmor : EquippableItem {

        public GhostArmor() : base(EquipType.ARMOR, 10, "Cursed Mail", "A cursed chainmail dropped by a spirit.") {
            Stats.Add(StatType.STRENGTH, -10);
            Stats.Add(StatType.AGILITY, -10);
        }
    }

    public class Shield : EquippableItem {

        public Shield() : base(Util.GetSprite("round-shield"), EquipType.OFFHAND, 0, "Basic Shield ", "A basic wooden shield.") {
            Stats.Add(StatType.AGILITY, -10);
            Stats.Add(StatType.VITALITY, 10);
        }

        public override Buff CreateBuff() {
            return new DamageResist();
        }
    }
}