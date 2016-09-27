using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WoodFan : EquippableItem {
    public WoodFan(int count)
        : base(
            "Wood Fan",
            "Drastically reduces stats.",
            count,
            EquipmentType.WEAPON,
            new Dictionary<AttributeType, PairedInt>() {
                { AttributeType.STRENGTH, new PairedInt(-Kitsune.STR + 6, 1) },
                { AttributeType.DEXTERITY, new PairedInt(-Kitsune.DEX + 2, 1) }
            }
            ) { }
}
