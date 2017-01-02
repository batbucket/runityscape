using System;
using System.Collections;
using System.Collections.Generic;

public class Party : IList<Character> {
    public Character Main;
    public IList<Character> Members; // PC also counts as a member
    public Inventory Inventory;

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

    public Party(Character pc) : this(pc, new Character[0]) { }

    public Party(Character pc, IList<Character> followers) {
        this.Main = pc;
        this.Inventory = pc.Inventory;

        this.Members = new List<Character>();
        Members.Add(pc);
        foreach (Character c in followers) {
            this.Add(c);
        }
    }

    public int IndexOf(Character item) {
        return Members.IndexOf(item);
    }

    public void Insert(int index, Character item) {
        item.Inventory = this.Inventory;
        Members.Insert(index, item);
    }

    public void RemoveAt(int index) {
        Members.RemoveAt(index);
    }

    public void Add(Character item) {
        item.Inventory = this.Inventory;
        Members.Add(item);
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
