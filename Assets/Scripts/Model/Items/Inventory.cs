using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * Is actually an ICollection<Item>, but we can't
 * Set it as that since Character's Selections
 * is an IDictionary<Selection, ICollection<Spell>>
 */
public class Inventory : ICollection<SpellFactory> {
    public const int CAPACITY = 11;

    public List<SpellFactory> Items { get; private set; }
    public int Count { get { return CalculateCount(); } }
    public bool IsFull { get { return Count >= CAPACITY; } }
    public bool IsReadOnly { get { return false; } }
    public bool IsEnabled { get; set; }

    public Inventory() {
        Items = new List<SpellFactory>();
        IsEnabled = true;
    }

    public void Add(Item item) {
        Util.Assert(item.Count > 0, "Attempted to add an item with Count of 0!");
        Util.Assert(!IsFull, "Attempted to add an item while Inventory was full!");
        if (IsFull) {
            return;
        } else if (!Items.Contains(item)) {
            Items.Add(item);
        } else {
            Find(item).Count += item.Count;
        }
    }

    public bool Remove(Item item) {
        SpellFactory res = Find(item);
        if (res == null) {
            return false;
        } else {
            res.Count--;
            if (res.Count <= 0) {
                Items.Remove(res);
            }
            return true;
        }
    }

    public int CalculateCount() {
        int count = 0;
        foreach (Item item in Items) {
            count += item.Count;
        }
        return count;
    }

    SpellFactory Find(SpellFactory item) {
        return Items.Find(x => x.Equals(item));
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

    IEnumerator IEnumerable.GetEnumerator() {
        return Items.GetEnumerator();
    }

    public void Add(SpellFactory spell) {
        Util.Assert(spell is Item);
        Add((Item)spell);
    }

    public bool Contains(SpellFactory spell) {
        Util.Assert(spell is Item);
        return Contains((Item)spell);
    }

    public void CopyTo(SpellFactory[] array, int arrayIndex) {
        Util.Assert(array is Item[]);
        CopyTo((SpellFactory[])array, arrayIndex);
    }

    public bool Remove(SpellFactory spell) {
        Util.Assert(spell is Item);
        return Remove((Item)spell);
    }

    public IEnumerator<SpellFactory> GetEnumerator() {
        return Items.GetEnumerator();
    }
}
