using Scripts.Model.Items;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using Scripts.Presenter;
using UnityEngine;
using Scripts.Model.Interfaces;
using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;

namespace Scripts.Model.Characters {

    /// <summary>
    /// An item pairing for easy initialization.
    /// </summary>
    public struct ItemPair {
        public readonly int Count;
        public readonly Item Item;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemPair"/> struct.
        /// </summary>
        /// <param name="count">The count of the item.</param>
        /// <param name="item">The item.</param>
        public ItemPair(int count, Item item) {
            this.Count = count;
            this.Item = item;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemPair"/> struct.
        /// </summary>
        /// <param name="item">The item.</param>
        public ItemPair(Item item) {
            this.Count = 1;
            this.Item = item;
        }
    }

    /// <summary>
    /// Represents a character's inventory. Parties have shared inventories.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Scripts.Model.Items.Item}" />
    /// <seealso cref="System.Collections.Generic.IEnumerable{Scripts.Model.Items.EquippableItem}" />
    /// <seealso cref="System.Collections.Generic.IEnumerable{Scripts.Model.Interfaces.ISpellable}" />
    /// <seealso cref="Scripts.Model.SaveLoad.ISaveable{Scripts.Model.SaveLoad.SaveObjects.InventorySave}" />
    public class Inventory : IEnumerable<Item>, IEnumerable<EquippableItem>, IEnumerable<ISpellable>, ISaveable<InventorySave> {

        /// <summary>
        /// The initial capacity of the inventory.
        /// </summary>
        public const int INITIAL_CAPACITY = 4;

        /// <summary>
        /// Internal dictionary for keeping track of item counts.
        /// </summary>
        private IDictionary<Item, int> dict;

        /// <summary>
        /// The maximum number of items this inventory can hold.
        /// </summary>
        private int capacity;

        /// <summary>
        /// Initializes a new instance of the <see cref="Inventory"/> class.
        /// </summary>
        public Inventory() {
            this.dict = new Dictionary<Item, int>();
            this.capacity = INITIAL_CAPACITY;
        }

        /// <summary>
        /// Initialize inventory with items.
        /// These starting items do not have to obey the capacity limit,
        /// but if the inventory is full/overfilled, new items cannot be added.
        /// </summary>
        /// <param name="initialItems">Initial items to add</param>
        public Inventory(params ItemPair[] initialItems) : this() {
            foreach (ItemPair item in initialItems) {
                dict.Add(item.Item, item.Count);
            }
        }

        public string WealthText {
            get {
                return string.Format("Wealth: {0}.", this.GetCount(new Game.Defined.Serialized.Items.Money()));
            }
        }

        /// <summary>
        /// Gets a value indicating whether this inventory is full.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is full; otherwise, <c>false</c>.
        /// </value>
        public bool IsFull {
            get {
                return TotalOccupiedSpace >= Capacity;
            }
        }

        /// <summary>
        /// Gets the total occupied space of this inventory.
        /// </summary>
        /// <value>
        /// The total occupied space.
        /// </value>
        public int TotalOccupiedSpace {
            get {
                int sum = 0;
                foreach (KeyValuePair<Item, int> pair in dict) {
                    if (pair.Key.HasFlag(Items.Flag.OCCUPIES_SPACE)) {
                        sum += pair.Value;
                    }
                }
                return sum;
            }
        }

        public string DetailedName {
            get {
                return string.Format("Items ({0}/{1})", TotalOccupiedSpace, Capacity);
            }
        }

        public string DetailedDescription {
            get {
                return string.Format("Use an item.\n{0} out of {1} inventory space is occupied.", TotalOccupiedSpace, Capacity);
            }
        }

        /// <summary>
        /// Gets the unique item count.
        /// </summary>
        /// <value>
        /// The unique item count.
        /// </value>
        public int UniqueItemCount {
            get {
                return dict.Count;
            }
        }

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        /// <value>
        /// The capacity.
        /// </value>
        public int Capacity {
            get {
                return capacity;
            }
            set {
                Util.Assert(value >= 0, "Value must be nonnegative.");
                capacity = value;
            }
        }

        /// <summary>
        /// Gets a fraction representing the inventory's occupied space and its capacity.
        /// </summary>
        /// <value>
        /// The fraction.
        /// </value>
        public string Fraction {
            get {
                return string.Format("{0}/{1}", TotalOccupiedSpace, Capacity);
            }
        }

        /// <summary>
        /// No capacity check
        /// </summary>
        public void ForceAdd(Item itemToAdd, int count = 1) {
            if (count > 0) {
                if (!dict.ContainsKey(itemToAdd)) {
                    dict.Add(itemToAdd, 0);
                }
                dict[itemToAdd] += count;
            }
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="itemToAdd">The item to add.</param>
        /// <param name="count">The count.</param>
        public void Add(Item itemToAdd, int count = 1) {
            Util.Assert(IsAddable(itemToAdd, count), "No more room!");
            Util.Assert(count >= 0, string.Format("Invalid count: {0}", count));
            ForceAdd(itemToAdd, count);
        }

        /// <summary>
        /// Determines whether the inventory has item of X amount.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="count">The count.</param>
        /// <returns>
        ///   <c>true</c> if the specified item has item; otherwise, <c>false</c>.
        /// </returns>
        public bool HasItem(Item item, int count = 1) {
            Util.Assert(count > 0, "Must use a positive count.");
            return dict.ContainsKey(item) && dict[item] >= count;
        }

        /// <summary>
        /// Converts the item's name to ItemName(NumberInInventory)
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public string CountedItemName(Item item) {
            return string.Format("{0}{1}",
                item.Name,
                GetCount(item) > 1 ? string.Format("({0})", GetCount(item)) : string.Empty
                );
        }

        /// <summary>
        /// Removes the specified item,
        /// </summary>
        /// <param name="itemToRemove">The item to remove.</param>
        /// <param name="count">The count.</param>
        public void Remove(Item itemToRemove, int count = 1) {
            Util.Assert(count >= 0, string.Format("Invalid count: {0}", count));
            if (dict.ContainsKey(itemToRemove)) {
                Util.Assert(IsRemovable(itemToRemove, count), string.Format("{0} only has quantity: {1}. Unable to remove {2} from it.", itemToRemove.Name, dict[itemToRemove], count));
                dict[itemToRemove] -= count;

                if (dict[itemToRemove] <= 0) {
                    dict.Remove(itemToRemove);
                }
            } else {
                Util.Assert(false, "Unable to find item.");
            }
        }

        /// <summary>
        /// Determines whether the specified item is addable.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="count">The count.</param>
        /// <returns>
        ///   <c>true</c> if the specified item is addable; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAddable(Item item, int count = 0) {
            return !item.HasFlag(Items.Flag.OCCUPIES_SPACE) || TotalOccupiedSpace < capacity;
        }

        /// <summary>
        /// Determines whether the specified item is removable.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="count">The count.</param>
        /// <returns>
        ///   <c>true</c> if the specified item is removable; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRemovable(Item item, int count = 0) {
            return dict[item] >= count;
        }

        /// <summary>
        /// Gets the number of item in the inventory.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public int GetCount(Item item) {
            return dict.ContainsKey(item) ? dict[item] : 0;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            var item = obj as Inventory;

            if (item == null) {
                return false;
            }

            return this.capacity.Equals(item.capacity)
                && Util.IsDictionariesEqual(this.dict, item.dict);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode() {
            return 0;
        }

        /// <summary>
        /// Gets the save object.
        /// </summary>
        /// <returns></returns>
        public InventorySave GetSaveObject() {
            List<InventorySave.ItemCount> list = new List<InventorySave.ItemCount>();
            foreach (KeyValuePair<Item, int> pair in dict) {
                list.Add(new InventorySave.ItemCount { Count = pair.Value, Item = new ItemSave(pair.Key.GetType()) });
            }

            return new InventorySave(capacity, list);
        }

        /// <summary>
        /// Initializes from save object.
        /// </summary>
        /// <param name="saveObject">The save object.</param>
        public void InitFromSaveObject(InventorySave saveObject) {
            this.capacity = saveObject.InventoryCapacity;
            foreach (InventorySave.ItemCount save in saveObject.Items) {
                Item item = save.Item.CreateObjectFromID();
                item.InitFromSaveObject(save.Item);
                dict.Add(item, save.Count);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return dict.Keys.GetEnumerator();
        }

        IEnumerator<ISpellable> IEnumerable<ISpellable>.GetEnumerator() {
            return dict.Keys.Cast<ISpellable>().ToList().GetEnumerator();
        }

        IEnumerator<Item> IEnumerable<Item>.GetEnumerator() {
            return dict.Keys.GetEnumerator();
        }

        IEnumerator<EquippableItem> IEnumerable<EquippableItem>.GetEnumerator() {
            return dict.Keys.Where(i => i is EquippableItem).Cast<EquippableItem>().GetEnumerator();
        }
    }
}