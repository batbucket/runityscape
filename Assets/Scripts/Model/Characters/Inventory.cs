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
    public struct ItemPair {
        public readonly int Count;
        public readonly Item Item;
        public ItemPair(int count, Item item) {
            this.Count = count;
            this.Item = item;
        }

        public ItemPair(Item item) {
            this.Count = 1;
            this.Item = item;
        }
    }

    public class Inventory : IEnumerable<Item>, IEnumerable<EquippableItem>, IEnumerable<ISpellable>, ISaveable<InventorySave> {
        private const int INITIAL_CAPACITY = 4;

        public Action<SplatDetails> AddSplat;

        private IDictionary<Item, int> dict;
        private int capacity;

        public Inventory() {
            this.dict = new Dictionary<Item, int>();
            this.AddSplat = ((p) => { });
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

        public bool IsFull {
            get {
                return TotalOccupiedSpace >= Capacity;
            }
        }

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

        public int UniqueItemCount {
            get {
                return dict.Count;
            }
        }

        public int Capacity {
            get {
                return capacity;
            }
            set {
                Util.Assert(value >= 0, "Value must be nonnegative.");
                capacity = value;
            }
        }

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

        public void Add(Item itemToAdd, int count = 1) {
            Util.Assert(IsAddable(itemToAdd, count), "No more room!");
            Util.Assert(count >= 0, string.Format("Invalid count: {0}", count));
            ForceAdd(itemToAdd, count);
            AddSplat(new SplatDetails(Color.green, string.Format("+{0}", count), itemToAdd.Icon));
        }

        public bool HasItem(Item item, int count = 1) {
            Util.Assert(count > 0, "Must use a positive count.");
            return dict.ContainsKey(item) && dict[item] >= count;
        }

        public string CountedItemName(Item item) {
            return string.Format("{0}{1}",
                item.Name,
                GetCount(item) > 1 ? string.Format("({0})", GetCount(item)) : string.Empty
                );
        }

        public void Remove(Item itemToRemove, int count = 1) {
            Util.Assert(count >= 0, string.Format("Invalid count: {0}", count));
            if (dict.ContainsKey(itemToRemove)) {
                Util.Assert(IsRemovable(itemToRemove, count), string.Format("{0} only has quantity: {1}. Unable to remove {2} from it.", itemToRemove.Name, dict[itemToRemove], count));
                dict[itemToRemove] -= count;

                if (dict[itemToRemove] <= 0) {
                    dict.Remove(itemToRemove);
                }

                AddSplat(new SplatDetails(Color.red, string.Format("-{0}", count), itemToRemove.Icon));

            } else {
                Util.Assert(false, "Unable to find item.");
            }
        }

        public bool IsAddable(Item item, int count = 0) {
            return !item.HasFlag(Items.Flag.OCCUPIES_SPACE) || TotalOccupiedSpace < INITIAL_CAPACITY;
        }

        public bool IsRemovable(Item item, int count = 0) {
            return dict[item] >= count;
        }

        public int GetCount(Item item) {
            return dict.ContainsKey(item) ? dict[item] : 0;
        }

        public override bool Equals(object obj) {
            var item = obj as Inventory;

            if (item == null) {
                return false;
            }

            return this.capacity.Equals(item.capacity)
                && Util.IsDictionariesEqual(this.dict, item.dict);
        }

        public override int GetHashCode() {
            return 0;
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

        public InventorySave GetSaveObject() {
            List<InventorySave.ItemCount> list = new List<InventorySave.ItemCount>();
            foreach (KeyValuePair<Item, int> pair in dict) {
                list.Add(new InventorySave.ItemCount { Count = pair.Value, Item = new ItemSave(pair.Key.GetType()) });
            }

            return new InventorySave(capacity, list);
        }

        public void InitFromSaveObject(InventorySave saveObject) {
            this.capacity = saveObject.InventoryCapacity;
            foreach (InventorySave.ItemCount save in saveObject.Items) {
                Item item = save.Item.CreateObjectFromID();
                item.InitFromSaveObject(save.Item);
                dict.Add(item, save.Count);
            }
        }

        IEnumerator<EquippableItem> IEnumerable<EquippableItem>.GetEnumerator() {
            return dict.Keys.Where(i => i is EquippableItem).Cast<EquippableItem>().GetEnumerator();
        }
    }
}
