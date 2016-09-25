using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaperFan : EquippableItem {
    public PaperFan(int count)
        : base(
            "PaperFan",
            "Drastically reduces stats.",
            count,
            EquipmentType.WEAPON,
            new Dictionary<AttributeType, PairedInt>() {
                { AttributeType.STRENGTH, new PairedInt(-Kitsune.STR + 5, 1) },
                { AttributeType.DEXTERITY, new PairedInt(-Kitsune.VIT + 1, 1) }
            }
            ) { }
}
