using Scripts.Model.Stats.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Model.Items {

    /// <summary>
    /// Represents a character's currently equipped items.
    /// </summary>
    public class Equipment : ICollection<EquippableItem> {
        public SortedDictionary<EquipmentType, EquippableItem> EquipD;

        public Equipment() {
            EquipD = new SortedDictionary<EquipmentType, EquippableItem>() {
            { EquipmentType.WEAPON, null },
            { EquipmentType.OFFHAND, null},
            { EquipmentType.ARMOR, null},
            { EquipmentType.TRINKET, null}
        };
        }

        public int Count { get { return EquipD.Values.Count(c => c != null); } }

        public bool IsReadOnly { get { return false; } }

        public void Add(EquippableItem equip) {
            EquipD[equip.EquipmentType] = equip;
        }

        public void Clear() {
            EquipD.Clear();
        }

        public bool Contains(EquippableItem equip) {
            return EquipD.ContainsValue(equip);
        }

        public bool ContainsEquipment(EquipmentType equipmentType) {
            return EquipD[equipmentType] != null;
        }

        public void CopyTo(EquippableItem[] array, int arrayIndex) {
            EquipD.Values.CopyTo(array, arrayIndex);
        }

        public EquippableItem Get(EquipmentType equipmentType) {
            return EquipD[equipmentType];
        }

        public int GetAttributeCount(AttributeType type) {
            int total = 0;
            foreach (EquippableItem e in this) {
                if (e != null && e.Bonuses.ContainsKey(type)) {
                    total += e.Bonuses[type];
                }
            }
            return total;
        }

        public IEnumerator<EquippableItem> GetEnumerator() {
            return EquipD.Values.GetEnumerator();
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

        IEnumerator IEnumerable.GetEnumerator() {
            return EquipD.Values.GetEnumerator();
        }
    }
}