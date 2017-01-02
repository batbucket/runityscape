using System.Collections.Generic;

public class Scimitar : EquippableItem {
    const string NAME = "Dragon Scimitar";
    const string DESCRIPTION = "A vicious, curved sword.";
    const EquipmentType EQUIPMENT_TYPE = EquipmentType.WEAPON;

    //Bonus stats go here
    static readonly IDictionary<AttributeType, int> ATTRIBUTE_BONUSES = new Dictionary<AttributeType, int>() {
        { AttributeType.STRENGTH, 300 },
        { AttributeType.AGILITY, 100 }
    };

    public Scimitar() : base(NAME, DESCRIPTION, EQUIPMENT_TYPE, ATTRIBUTE_BONUSES) { }
}