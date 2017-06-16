using Scripts.Model.Items;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using Scripts.Presenter;
using UnityEngine;
using Scripts.Model.Spells;

namespace Scripts.Model.Characters {

    public class Inventory : IEnumerable<Item>, IEnumerable<SpellBook> {
        private struct ItemIndex {
            public readonly Item Item;
            public readonly int Index;
            public ItemIndex(Item item, int index) {
                this.Item = item;
                this.Index = index;
            }
        }

        private const int CAPACITY = 10;

        public Action<SplatDetails> AddSplat;

        private List<Item> list;

        private IDictionary<Item, Stack<ItemIndex>> dict;
        private int size;

        public Inventory() {
            this.list = new List<Item>();
            this.dict = new Dictionary<Item, Stack<ItemIndex>>();
            this.AddSplat = ((p) => { });
        }

        public IList<Item> List {
            get {
                return list;
            }
        }

        public int Count {
            get {
                return List.Count;
            }
        }

        public void Add(Item itemToAdd) {
            Util.Assert(IsAddable(itemToAdd), "No more room!");

            // Update last added item of same type
            if (HasItem(itemToAdd)) {
                GetLastItem(itemToAdd).Add(itemToAdd);
            }

            // Put remainder into a new item slot
            if (itemToAdd.Count > 0) {
                int index = List.Count;
                List.Add(itemToAdd);
                PushStack(itemToAdd, index);
            }

            AddSplat(new SplatDetails(Color.white, string.Format("+{0}", itemToAdd.Name), itemToAdd.Icon));
        }

        public bool HasItem(Item item) {
            return dict.ContainsKey(item) && dict[item].Count > 0;
        }

        public void Remove(Item itemToRemove) {
            if (dict.ContainsKey(itemToRemove)) {

                // Remove from topmost item
                GetLastItem(itemToRemove).Remove(itemToRemove);

                // Pop if item is empty, remove from list
                if (GetLastItem(itemToRemove).Count == 0) {
                    ItemIndex i = PopStack(itemToRemove);

                    //Remove index
                    List.RemoveAt(i.Index);
                }

                // Remove from the next item
                if (HasItem(itemToRemove)) {
                    GetLastItem(itemToRemove).Remove(itemToRemove);
                }

                AddSplat(new SplatDetails(Color.white, string.Format("-{0}", itemToRemove.Name), itemToRemove.Icon));

            } else {
                Util.Assert(false, "Unable to find item.");
            }
        }

        public bool IsAddable(Item itemToAdd) {
            return (Count < CAPACITY) || (GetLastItem(itemToAdd).RemainingSpace >= itemToAdd.Count);
        }

        public int GetCount(Item item) {
            int count = 0;
            if (!dict.ContainsKey(item)) {
                count = 0;
            } else {
                Stack<ItemIndex> s = GetStack(item);
                foreach (ItemIndex i in s) {
                    count += i.Item.Count;
                }
            }
            return count;
        }

        private Item GetLastItem(Item item) {
            return dict[item].Peek().Item;
        }

        private Stack<ItemIndex> GetStack(Item item) {
            if (!dict.ContainsKey(item)) {
                dict.Add(item, new Stack<ItemIndex>());
            }
            return dict[item];
        }

        private ItemIndex PopStack(Item item) {
            Util.Assert(dict.ContainsKey(item) && dict[item].Count > 0 && GetLastItem(item).Count == 0, "Unable to pop.");
            return dict[item].Pop();
        }

        private void PushStack(Item item, int index) {
            Util.Assert(!HasItem(item) || GetLastItem(item).IsFull, "Last item not at full capacity.");
            GetStack(item).Push(new ItemIndex(item, index));
        }

        IEnumerator<Item> IEnumerable<Item>.GetEnumerator() {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return list.GetEnumerator();
        }

        IEnumerator<SpellBook> IEnumerable<SpellBook>.GetEnumerator() {
            return list.Where(i => i is UseableItem).Cast<UseableItem>().Select(i => i.GetSpellBook()).ToList().GetEnumerator();
        }
    }
}
