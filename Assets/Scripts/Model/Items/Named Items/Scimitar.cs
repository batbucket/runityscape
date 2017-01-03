using Scripts.Model.Stats.Attributes;
using System.Collections.Generic;

namespace Scripts.Model.Items.Named {

    public class Scimitar : EquippableItem {
        private const string NAME = "Dragon Scimitar";
        private const string DESCRIPTION = "A vicious, curved sword.";
        private const EquipmentType EQUIPMENT_TYPE = EquipmentType.WEAPON;

        //Bonus stats go here
        private static readonly IDictionary<AttributeType, int> ATTRIBUTE_BONUSES = new Dictionary<AttributeType, int>() {
        { AttributeType.STRENGTH, 300 },
        { AttributeType.AGILITY, 100 }
    };

        public Scimitar() : base(NAME, DESCRIPTION, EQUIPMENT_TYPE, ATTRIBUTE_BONUSES) {
        }
    }
}