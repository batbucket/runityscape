using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory {
    List<Item> itemList;
    public const int CAPACITY = 11;
    public Inventory() {
        itemList = new List<Item>();
    }

    public bool add(Item item) {
        if (isFull()) {
            return false;
        } else if (!itemList.Contains(item)) {
            itemList.Add(item);
        } else {
            find(item).addCount(item.getCount());
        }
        return true;
    }

    public bool remove(Item item) {
        if (!itemList.Contains(item)) {
            return false;
        } else if (find(item).getCount() > 1) {
            find(item).decCount();
        } else {
            item.decCount();
            itemList.Remove(item);
        }
        return true;
    }

    public int getCount() {
        int count = 0;
        foreach (Item item in itemList) {
            count += item.getCount();
        }
        return count;
    }

    public List<Item> getList() {
        return itemList;
    }

    public bool isFull() {
        return getCount() >= CAPACITY;
    }

    public bool hasItem(Item item) {
        return find(item) != null;
    }

    Item find(Item item) {
        return itemList.Find(x => x.Equals(item));
    }
}
