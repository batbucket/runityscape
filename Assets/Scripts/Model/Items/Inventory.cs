using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * Is actually an ICollection<Item>, but we can't
 * Set it as that since Character's Selections
 * is an IDictionary<Selection, ICollection<Spell>>
 */
public class Inventory : ICollection<Item> {
    public const int CAPACITY = 11;

    public List<Item> Items { get; private set; }
    public int money;
    public bool IsFull { get { return Count >= CAPACITY; } }
    public bool IsReadOnly { get { return false; } }
    public bool IsEnabled { get; set; }
    public int Count { get { return Items.Count; } }

    public Inventory() {
        Items = new List<Item>();
        IsEnabled = true;
    }

    public void Add(Item item) {
        Util.Assert(!IsFull, "Attempted to add an item while Inventory was full!");
        if (IsFull) {
            return;
        } else {
            Items.Add(item);
        }
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

    private Item Find(Item item) {
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

    public IEnumerator<Item> GetEnumerator() {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return Items.GetEnumerator();
    }
}
