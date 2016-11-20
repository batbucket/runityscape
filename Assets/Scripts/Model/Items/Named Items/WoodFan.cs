using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WoodFan : EquippableItem {
    public WoodFan()
        : base(
            "Wood Fan",
            "Drastically reduces stats.",
            EquipmentType.WEAPON,
            new Dictionary<AttributeType, PairedInt>() {
                { AttributeType.STRENGTH, new PairedInt(-Kitsune.STR + 4, 1) },
                { AttributeType.DEXTERITY, new PairedInt(-Kitsune.DEX + 2, 1) },
                { AttributeType.VITALITY, new PairedInt(-Kitsune.VIT + 10, 1) }
            }
            ) { }
}
