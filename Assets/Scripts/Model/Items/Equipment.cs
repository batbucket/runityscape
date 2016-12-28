using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Equipment : ICollection<EquippableItem> {
    public SortedDictionary<EquipmentType, EquippableItem> EquipD;

    public int Count { get { return EquipD.Values.Count(c => c != null); } }

    public bool IsReadOnly { get { return false; } }

    public Equipment(Inventory inventory) {
        EquipD = new SortedDictionary<EquipmentType, EquippableItem>() {
            { EquipmentType.WEAPON, null },
            { EquipmentType.OFFHAND, null},
            { EquipmentType.ARMOR, null},
            { EquipmentType.TRINKET, null}
        };
    }

    public EquippableItem Get(EquipmentType equipmentType) {
        return EquipD[equipmentType];
    }

    public bool ContainsEquipment(EquipmentType equipmentType) {
        return EquipD[equipmentType] != null;
    }

    public void Add(EquippableItem equip) {
        EquipD[equip.EquipmentType] = equip;
    }

    public void Clear() {
        EquipD.Clear();
    }

    public bool Contains(EquippableItem equip) {
        return EquipD.ContainsValue(equip);
    }

    public bool Remove(EquippableItem equip) {
        EquippableItem unequip = EquipD[equip.EquipmentType];
        if (equip.Equals(unequip)) {
            EquipD[equip.EquipmentType] = null;
            return true;
        } else {
            return false;
        }
    }

    public override string ToString() {
        return string.Join(", ", EquipD.Values.Where(e => e != null).Select(e => e.Name).ToArray());
    }

    public void CopyTo(EquippableItem[] array, int arrayIndex) {
        EquipD.Values.CopyTo(array, arrayIndex);
    }

    public IEnumerator<EquippableItem> GetEnumerator() {
        return EquipD.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return EquipD.Values.GetEnumerator();
    }
}
