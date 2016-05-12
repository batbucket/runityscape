using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Equipment : ICollection<SpellFactory> {
    SortedDictionary<EquipmentType, EquippableItem> equipment;
    Inventory inventory;

    public int Count { get { return equipment.Count; } }

    public bool IsReadOnly { get { return false; } }

    public Equipment(Inventory inventory) {
        this.inventory = inventory;
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

    public void Add(SpellFactory myItem) {
        Util.Assert(myItem is EquippableItem);
        EquippableItem item = (EquippableItem)myItem;

        ConstructorInfo ctor = item.GetType().GetConstructor(new[] { typeof(int) });
        EquippableItem equip = (EquippableItem)ctor.Invoke(new object[] { 1 });
        EquippableItem unequip = equipment[equip.EquipmentType];
        if (unequip != null) {
            inventory.Add(unequip);
        }
        equipment[equip.EquipmentType] = equip;
    }

    public void Clear() {
        equipment.Clear();
    }

    public bool Contains(SpellFactory item) {
        Util.Assert(item is EquippableItem);
        return equipment.ContainsValue((EquippableItem)item);
    }

    public void CopyTo(SpellFactory[] array, int arrayIndex) {
        equipment.Values.Cast<SpellFactory>().ToArray().CopyTo(array, arrayIndex);
    }

    public bool Remove(SpellFactory item) {
        Util.Assert(item is EquippableItem);
        EquippableItem equip = (EquippableItem)item;
        EquippableItem unequip = equipment[equip.EquipmentType];
        if (equip.Equals(unequip)) {
            inventory.Add(unequip);
            equipment[equip.EquipmentType] = null;
            return true;
        } else {
            return false;
        }
    }

    public IEnumerator<SpellFactory> GetEnumerator() {
        return equipment.Values.Select(e => (SpellFactory)e).OfType<SpellFactory>().ToList().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return equipment.Values.GetEnumerator();
    }
}
