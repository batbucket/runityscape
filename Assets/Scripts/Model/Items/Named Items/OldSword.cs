using Scripts.Model.Stats.Attributes;
using System.Collections.Generic;

namespace Scripts.Model.Items.Named {

    public class OldSword : EquippableItem {
        private const string NAME = "Old Sword";
        private const string DESCRIPTION = "A familiar sword from an old time.";
        private const EquipmentType EQUIPMENT_TYPE = EquipmentType.WEAPON;

        //Bonus stats go here
        private static readonly IDictionary<AttributeType, int> ATTRIBUTE_BONUSES = new Dictionary<AttributeType, int>() {
        { AttributeType.STRENGTH, 2 },
        { AttributeType.AGILITY, 1 }
    };

        public OldSword() : base(NAME, DESCRIPTION, EQUIPMENT_TYPE, ATTRIBUTE_BONUSES) {
        }
    }
}