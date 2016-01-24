using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Inventory : ICollection<Item> {
    public const int CAPACITY = 11;

    public List<Item> Items { get; private set; }
    public int Count { get { return CalculateCount(); } }
    public bool IsFull { get { return Count >= CAPACITY; } }
    public bool IsReadOnly { get { return false; } }
    public bool IsEnabled { get; set; }

    public Inventory() {
        Items = new List<Item>();
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
        Item res = Find(item);
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

    Item Find(Item item) {
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
