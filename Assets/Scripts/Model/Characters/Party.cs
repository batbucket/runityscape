using System.Collections;
using System.Collections.Generic;

namespace Scripts.Model.Characters {

    /// <summary>
    /// Represents a grouping of the user's Characters.
    /// Everyone's inventory references the leader's inventory upon
    /// being added to a party.
    /// </summary>
    public class Party : IList<Character> {
        public Character Leader;
        public IList<Character> Members; // Leader also counts as a member

        public int Count {
            get {
                return Members.Count;
            }
        }

        public bool IsReadOnly {
            get {
                return Members.IsReadOnly;
            }
        }

        public Character this[int index] {
            get {
                return Members[index];
            }

            set {
                Members[index] = value;
            }
        }

        public Party() {
            this.Leader = null;
            this.Members = new List<Character>();
        }

        public Party(Character pc) : this(pc, new Character[0]) {
        }

        public Party(Character leader, IList<Character> followers) {
            this.Leader = leader;
            this.Members = new List<Character>();
            Members.Add(leader);
            foreach (Character c in followers) {
                this.Add(c);
            }
        }

        public int IndexOf(Character item) {
            return Members.IndexOf(item);
        }

        public void Insert(int index, Character item) {
            Members.Insert(index, item);
        }

        public void RemoveAt(int index) {
            Members.RemoveAt(index);
        }

        public void Add(Character item) {
            if (!Members.Contains(item)) {
                Members.Add(item);
            }
        }

        public void Clear() {
            Members.Clear();
        }

        public bool Contains(Character item) {
            return Members.Contains(item);
        }

        public void CopyTo(Character[] array, int arrayIndex) {
            Members.CopyTo(array, arrayIndex);
        }

        public bool Remove(Character item) {
            return Members.Remove(item);
        }

        public IEnumerator<Character> GetEnumerator() {
            return Members.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Members.GetEnumerator();
        }
    }
}