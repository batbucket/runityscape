using Scripts.Model.Items;
using Scripts.Model.Stats;

namespace Scripts.Game.Defined.Serialized.Items {

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
}