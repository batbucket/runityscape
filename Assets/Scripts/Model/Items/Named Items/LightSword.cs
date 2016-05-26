using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightSword : EquippableItem {
    const string NAME = "Light Sword";
    const string DESCRIPTION = "A mass-produced sword made from a light material.";
    const EquipmentType EQUIPMENT_TYPE = EquipmentType.WEAPON;

    //Bonus stats go here
    static readonly IDictionary<AttributeType, PairedInt> ATTRIBUTE_BONUSES = new Dictionary<AttributeType, PairedInt>() {
        { AttributeType.STRENGTH, new PairedInt(3, 1) },
        { AttributeType.DEXTERITY, new PairedInt(1, 1) }
    };

    public LightSword(int count) : base(NAME, DESCRIPTION, count, EQUIPMENT_TYPE, ATTRIBUTE_BONUSES) { }
}
