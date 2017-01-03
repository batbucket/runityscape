using Scripts.Model.Items;
using System.Collections.Generic;

namespace Scripts.Model.Characters {

    /// <summary>
    /// Helper struct for passing inventory items to a Character.
    /// Also allows equipment to be equipped without affecting LIFE percentage.
    ///
    /// Normal equipping: Character has 10/10 LIFE, equips armor, now has 10/20 LIFE
    /// Equipping through constructor: Character has 10/10 LIFE, equips armor, now has 20/20 LIFE
    /// </summary>
    public struct Items {
        public IDictionary<EquipmentType, EquippableItem> Equips;
        public Item[] Inventory;

        public Items(Item[] items, EquippableItem[] equips) {
            this.Inventory = items;

            this.Equips = new Dictionary<EquipmentType, EquippableItem>();
            for (int i = 0; i < equips.Length; i++) {
                EquippableItem e = equips[i];
                this.Equips.Add(e.EquipmentType, e);
            }
        }

        public Items(Item[] items) : this(items, new EquippableItem[0]) {
        }

        public Items(EquippableItem[] equips) : this(new Item[0], equips) {
        }
    }
}