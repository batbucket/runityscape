using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory {
    Dictionary<Item, int> itemList;
    public const int CAPACITY = 11;
    public Inventory() {
        itemList = new Dictionary<Item, int>();
    }

    public bool add(Item item) {
        if (isFull()) {
            return false;
        } else if (!itemList.ContainsKey(item)) {
            itemList.Add(item, 1);
        } else {
            itemList[item]++;
        }
            return true;
    }

    public bool remove(Item item) {
        if (!itemList.ContainsKey(item)) {
            return false;
        } else if (itemList[item] > 1) {
            itemList[item]--;
        } else {
            itemList.Remove(item);
        }
        return true;
    }

    public int getCount() {
        int count = 0;
        foreach (KeyValuePair<Item, int> k in itemList) {
            count += k.Value;
        }
        return count;
    }

    public Dictionary<Item, int> getDictionary() {
        return itemList;
    }

    public bool isFull() {
        return getCount() >= CAPACITY;
    }
}
