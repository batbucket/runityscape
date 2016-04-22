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

    public EquippableItem Equip(EquipmentType equipmentType, EquippableItem equippableItem) {
        EquippableItem current = equipment[equipmentType];
        equipment[equipmentType] = equippableItem;
        return current;
    }

    public EquippableItem Unequip(EquipmentType equipmentType) {
        return Equip(equipmentType, null);
    }

    public bool HasEquipment(EquipmentType equipmentType) {
        return equipment.ContainsKey(equipmentType);
    }
}
