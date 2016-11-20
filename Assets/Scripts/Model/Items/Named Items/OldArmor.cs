using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OldArmor : EquippableItem {
    const string NAME = "Old Armor";
    const string DESCRIPTION = "A familiar armor from an old time.";
    const EquipmentType EQUIPMENT_TYPE = EquipmentType.ARMOR;

    //Bonus stats go here
    static readonly IDictionary<AttributeType, PairedInt> ATTRIBUTE_BONUSES = new Dictionary<AttributeType, PairedInt>() {
        { AttributeType.VITALITY, new PairedInt(2, 1) }
    };

    public OldArmor() : base(NAME, DESCRIPTION, EQUIPMENT_TYPE, ATTRIBUTE_BONUSES) { }
}
