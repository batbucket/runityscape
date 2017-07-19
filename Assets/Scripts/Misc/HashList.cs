using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// List that only allows one of the same element.
/// </summary>
public class HashList<T> : IList<T> {
    private List<T> list;
    private HashSet<T> set;

    public HashList(IEqualityComparer<T> comparer) {
        this.list = new List<T>();
        this.set = new HashSet<T>(comparer);
    }

    public T this[int index] {
        get {
            return list[index];
        }

        set {
            if (!set.Contains(value)) {
                this.list[index] = value;
            }
            set.Add(value);
        }
    }

    public int Count {
        get {
            return list.Count;
        }
    }

    public bool IsReadOnly {
        get {
            return false;
        }
    }

    public void Shuffle() {
        this.list.Shuffle();
    }

    public void Add(T item) {
        if (!set.Contains(item)) {
            this.list.Add(item);
        }
        set.Add(item);
    }

    public void Clear() {
        list.Clear();
        set.Clear();
    }

    public bool Contains(T item) {
        return set.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex) {
        list.CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator() {
        return list.GetEnumerator();
    }

    public int IndexOf(T item) {
        return list.IndexOf(item);
    }

    public void Insert(int index, T item) {
        this[index] = item;
    }

    public bool Remove(T item) {
        this.list.Remove(item);
        return this.set.Remove(item);
    }

    public void RemoveAt(int index) {
        T item = list[index];
        this.list.RemoveAt(index);
        set.Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return list.GetEnumerator();
    }
}
