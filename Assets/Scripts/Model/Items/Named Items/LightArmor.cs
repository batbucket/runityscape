using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightArmor : EquippableItem {
    const string NAME = "Light Armor";
    const string DESCRIPTION = "A mass-produced armor made from a light material.";
    const EquipmentType EQUIPMENT_TYPE = EquipmentType.ARMOR;

    //Bonus stats go here
    static readonly IDictionary<AttributeType, PairedInt> ATTRIBUTE_BONUSES = new Dictionary<AttributeType, PairedInt>() {
        { AttributeType.DEXTERITY, new PairedInt(1, 1) }
    };

    public LightArmor(int count) : base(NAME, DESCRIPTION, count, EQUIPMENT_TYPE, ATTRIBUTE_BONUSES) { }
}
