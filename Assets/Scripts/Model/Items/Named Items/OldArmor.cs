using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OldArmor : EquippableItem {
    const string NAME = "Old Armor";
    const string DESCRIPTION = "A familiar armor from an old time.";
    const EquipmentType EQUIPMENT_TYPE = EquipmentType.ARMOR;

    //Bonus stats go here
    static readonly IDictionary<AttributeType, int> ATTRIBUTE_BONUSES = new Dictionary<AttributeType, int>() {
        { AttributeType.VITALITY, 2 }
    };

    public OldArmor() : base(NAME, DESCRIPTION, EQUIPMENT_TYPE, ATTRIBUTE_BONUSES) { }
}