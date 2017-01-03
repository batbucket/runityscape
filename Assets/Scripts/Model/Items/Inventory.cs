using System.Collections;
using System.Collections.Generic;

namespace Scripts.Model.Items {

    /// <summary>
    /// A Character's items and gold.
    /// </summary>
    public class Inventory : ICollection<Item> {
        public const int CAPACITY = 11;

        public int Gold;

        public Inventory() {
            Items = new List<Item>();
            IsEnabled = true;
        }

        public int Count { get { return Items.Count; } }
        public bool IsEnabled { get; set; }
        public bool IsFull { get { return Count >= CAPACITY; } }
        public bool IsReadOnly { get { return false; } }
        public List<Item> Items { get; private set; }

        public string SizeText {
            get {
                return string.Format("Inventory size: {0}/{1}.", Count, CAPACITY);
            }
        }

        public void Add(Item item) {
            Util.Assert(!IsFull, "Attempted to add an item while Inventory was full!");
            if (IsFull) {
                return;
            } else {
                Items.Add(item);
            }
        }

        public void Clear() {
            Items.Clear();
        }

        public bool Contains(Item item) {
            return Items.Contains(item);
        }

        public void CopyTo(Item[] array, int arrayIndex) {
            Items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Item> GetEnumerator() {
            return Items.GetEnumerator();
        }

        public bool Remove(Item item) {
            Item res = Find(item);
            if (res == null) {
                return false;
            } else {
                Items.Remove(item);
                return true;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Items.GetEnumerator();
        }

        private Item Find(Item item) {
            return Items.Find(x => x.Equals(item));
        }
    }
}