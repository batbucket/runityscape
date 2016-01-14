using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Equipment {
    Dictionary<EquipmentType, EquippableItem> equipment;

	public Equipment() {
        equipment = new Dictionary<EquipmentType, EquippableItem>();
        equipment.Add(EquipmentType.WEAPON, null);
        equipment.Add(EquipmentType.OFFHAND, null);
        equipment.Add(EquipmentType.ARMOR, null);
        equipment.Add(EquipmentType.TRINKET, null);
    }
}
