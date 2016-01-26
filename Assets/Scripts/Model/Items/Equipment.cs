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

    public Equipment Equip(EquipmentType equipmentType, EquippableItem equippableItem) {
        return null;
    }

    public Equipment Unequip(EquipmentType equipmentType) {
        return null;
    }

    public bool HasEquipment(EquipmentType equipmentType) {
        return false;
    }
}
