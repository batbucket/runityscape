using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OldSword : EquippableItem {
    const string NAME = "Old Sword";
    const string DESCRIPTION = "A familiar sword from an old time.";
    const EquipmentType EQUIPMENT_TYPE = EquipmentType.WEAPON;

    //Bonus stats go here
    static readonly IDictionary<AttributeType, int> ATTRIBUTE_BONUSES = new Dictionary<AttributeType, int>() {
        { AttributeType.STRENGTH, 2 },
        { AttributeType.AGILITY, 1 }
    };

    public OldSword() : base(NAME, DESCRIPTION, EQUIPMENT_TYPE, ATTRIBUTE_BONUSES) { }
}
