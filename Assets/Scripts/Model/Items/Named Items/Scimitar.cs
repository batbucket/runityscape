using System.Collections.Generic;

public class Scimitar : EquippableItem {
    const string NAME = "Dragon Scimitar";
    const string DESCRIPTION = "A vicious, curved sword.";
    const EquipmentType EQUIPMENT_TYPE = EquipmentType.WEAPON;

    //Bonus stats go here
    static readonly IDictionary<AttributeType, PairedInt> ATTRIBUTE_BONUSES = new Dictionary<AttributeType, PairedInt>() {
        { AttributeType.STRENGTH, new PairedInt(3, 1) },
        { AttributeType.DEXTERITY, new PairedInt(5, 1) }
    };

    public Scimitar(int count) : base(NAME, DESCRIPTION, count, EQUIPMENT_TYPE, ATTRIBUTE_BONUSES) { }
}