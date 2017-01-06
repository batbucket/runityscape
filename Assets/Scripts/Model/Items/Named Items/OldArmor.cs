using Scripts.Model.Stats.Attributes;
using System.Collections.Generic;

namespace Scripts.Model.Items.Named {

    public class OldArmor : EquippableItem {
        private const string NAME = "Old Armor";
        private const string DESCRIPTION = "A familiar armor from an old time.";
        private const EquipmentType EQUIPMENT_TYPE = EquipmentType.ARMOR;

        //Bonus stats go here
        private static readonly IDictionary<AttributeType, int> ATTRIBUTE_BONUSES = new Dictionary<AttributeType, int>() {
        { AttributeType.VITALITY, 2 },
        { AttributeType.AGILITY, -1 }
    };

        public OldArmor() : base(NAME, DESCRIPTION, EQUIPMENT_TYPE, ATTRIBUTE_BONUSES) {
        }
    }
}