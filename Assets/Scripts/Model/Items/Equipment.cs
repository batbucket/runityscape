using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Equipment {
    SortedDictionary<EquipmentType, EquippableItem> equipment;

	public Equipment() {
        equipment = new SortedDictionary<EquipmentType, EquippableItem>();
        equipment.Add(EquipmentType.WEAPON, null);
        equipment.Add(EquipmentType.OFFHAND, null);
        equipment.Add(EquipmentType.ARMOR, null);
        equipment.Add(EquipmentType.TRINKET, null);
    }

    public Equipment equip(EquipmentType equipmentType, EquippableItem equippableItem) {
        return null;
    }

    public Equipment unequip(EquipmentType equipmentType) {
        return null;
    }

    public bool hasEquipment(EquipmentType equipmentType) {
        return false;
    }
}
