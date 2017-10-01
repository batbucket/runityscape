using Scripts.Game.Defined.Serialized.Buffs;
using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Model.Buffs;
using Scripts.Model.Items;
using Scripts.Model.Stats;

namespace Scripts.Game.Defined.Serialized.Items {

    public class PoisonArmor : EquippableItem {

        public PoisonArmor() : base(EquipType.ARMOR, 10, "Poisoned Armor", "This doesn't look safe.") {
            AddFlatStatBonus(StatType.VITALITY, 3);
            AddFlatStatBonus(StatType.AGILITY, -1);
        }

        public override Buff CreateBuff() {
            return new Poison();
        }
    }

    public class BrokenSword : EquippableItem {

        public BrokenSword() : base(EquipType.WEAPON, 5, "Broken Sword", "A broken sword dropped by a spirit.") {
            AddFlatStatBonus(StatType.STRENGTH, 1);
            AddFlatStatBonus(StatType.VITALITY, -4);
        }
    }

    public class GhostArmor : EquippableItem {

        public GhostArmor() : base(EquipType.ARMOR, 10, "Cursed Mail", "A cursed chainmail dropped by a spirit.") {
            AddFlatStatBonus(StatType.STRENGTH, -10);
            AddFlatStatBonus(StatType.AGILITY, -10);
        }
    }

    public class Shield : EquippableItem {

        public Shield() : base(Util.GetSprite("round-shield"), EquipType.OFFHAND, 0, "Basic Shield ", "A basic wooden shield.") {
            AddFlatStatBonus(StatType.AGILITY, -10);
            AddFlatStatBonus(StatType.VITALITY, 10);
        }

        public override Buff CreateBuff() {
            return new DamageResist();
        }
    }

    public class FishHook : EquippableItem {

        public FishHook() : base(EquipType.WEAPON, 50, "Fish Hook", "A used fish hook.") {
            AddFlatStatBonus(StatType.STRENGTH, 5);
            AddFlatStatBonus(StatType.AGILITY, 1);
            AddFlatStatBonus(StatType.VITALITY, -1);
        }

        //if used on fish creature, more effective??
    }

    public class RegenerationArmor : EquippableItem {

        public RegenerationArmor() : base(EquipType.ARMOR, 10, "Regeneration Armor", "Soothing armor that heals wounds.") {
            AddFlatStatBonus(StatType.AGILITY, -2);
            AddFlatStatBonus(StatType.VITALITY, 5);
        }

        public override Buff CreateBuff() {
            return new Restore();
        }
    }
}