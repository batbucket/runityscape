using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OldSword : EquippableItem {
    const string NAME = "Old Sword";
    const string DESCRIPTION = "A familiar sword from an old time.";
    const EquipmentType EQUIPMENT_TYPE = EquipmentType.WEAPON;

    //Bonus stats go here
    static readonly IDictionary<AttributeType, PairedInt> ATTRIBUTE_BONUSES = new Dictionary<AttributeType, PairedInt>() {
        { AttributeType.STRENGTH, new PairedInt(2, 1) },
        { AttributeType.AGILITY, new PairedInt(2, 1) }
    };

    public OldSword() : base(NAME, DESCRIPTION, EQUIPMENT_TYPE, ATTRIBUTE_BONUSES) { }
}
