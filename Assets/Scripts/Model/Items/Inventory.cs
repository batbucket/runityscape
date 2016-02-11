using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * Is actually an ICollection<Item>, but we can't
 * Set it as that since Character's Selections
 * is an IDictionary<Selection, ICollection<Spell>>
 */
public class Inventory : ICollection<Spell> {
    public const int CAPACITY = 11;

    public List<Spell> Items { get; private set; }
    public int Count { get { return CalculateCount(); } }
    public bool IsFull { get { return Count >= CAPACITY; } }
    public bool IsReadOnly { get { return false; } }
    public bool IsEnabled { get; set; }

    public Inventory() {
        Items = new List<Spell>();
        IsEnabled = true;
    }

    public void Add(Item item) {
        if (IsFull) {
            return;
        } else if (!Items.Contains(item)) {
            Items.Add(item);
        } else {
            Find(item).Count += item.Count;
        }
    }

    public bool Remove(Item item) {
        Spell res = Find(item);
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

    Spell Find(Spell item) {
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

    /**
     * Inventory needs to be in the Character's Selections
     * Selections is an ICollection<Selection, ICollection<Spell>>
     * So we need to implement ICollection<Spell> in Inventory
     * If we keep this here, we can implement ICollection<Spell>
     * In inventory, and force the caller to only add Items to the collection.
     * This is way better than the alternative.
     */

    public void Add(Spell spell) {
        Util.Assert(spell is Item);
        Add((Item)spell);
    }

    public bool Contains(Spell spell) {
        Util.Assert(spell is Item);
        return Contains((Item)spell);
    }

    public void CopyTo(Spell[] array, int arrayIndex) {
        Util.Assert(array is Item[]);
        CopyTo((Spell[])array, arrayIndex);
    }

    public bool Remove(Spell spell) {
        Util.Assert(spell is Item);
        return Remove((Item)spell);
    }

    public IEnumerator<Spell> GetEnumerator() {
        return Items.GetEnumerator();
    }
}
