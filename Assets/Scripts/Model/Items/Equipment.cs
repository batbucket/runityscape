using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Equipment : ICollection<EquippableItem> {
    private SortedDictionary<EquipmentType, EquippableItem> equipment;

    public int Count { get { return equipment.Values.Count(c => c != null); } }

    public bool IsReadOnly { get { return false; } }

    public Equipment(Inventory inventory) {
        equipment = new SortedDictionary<EquipmentType, EquippableItem>() {
            { EquipmentType.WEAPON, null },
            { EquipmentType.OFFHAND, null},
            { EquipmentType.ARMOR, null},
            { EquipmentType.TRINKET, null}
        };
    }

    public EquippableItem Get(EquipmentType equipmentType) {
        return equipment[equipmentType];
    }

    public bool ContainsEquipment(EquipmentType equipmentType) {
        return equipment[equipmentType] != null;
    }

    public void Add(EquippableItem equip) {
        equipment[equip.EquipmentType] = equip;
    }

    public void Clear() {
        equipment.Clear();
    }

    public bool Contains(EquippableItem equip) {
        return equipment.ContainsValue(equip);
    }

    public bool Remove(EquippableItem equip) {
        EquippableItem unequip = equipment[equip.EquipmentType];
        if (equip.Equals(unequip)) {
            equipment[equip.EquipmentType] = null;
            return true;
        } else {
            return false;
        }
    }

    public override string ToString() {
        return string.Join(", ", equipment.Values.Where(e => e != null).Select(e => e.Name).ToArray());
    }

    public void CopyTo(EquippableItem[] array, int arrayIndex) {
        equipment.Values.CopyTo(array, arrayIndex);
    }

    public IEnumerator<EquippableItem> GetEnumerator() {
        return equipment.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return equipment.Values.GetEnumerator();
    }
}
