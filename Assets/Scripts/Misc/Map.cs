using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map<A, B> : IEnumerable<KeyValuePair<A, B>> {
    private IDictionary<A, B> dict1;
    private IDictionary<B, A> dict2;

    public Map() {
        this.dict1 = new Dictionary<A, B>();
        this.dict2 = new Dictionary<B, A>();
    }

    public ICollection<A> ACollection {
        get {
            return dict1.Keys;
        }
    }

    public ICollection<B> BCollection {
        get {
            return dict2.Keys;
        }
    }

    public void Add(A key1, B key2) {
        Util.Assert(!Contains(key1) && !Contains(key2), string.Format("Duplicate elements detected: <{0}, {1}>", key1.ToString(), key2.ToString()));
        dict1.Add(key1, key2);
        dict2.Add(key2, key1);
    }

    public B Get(A value) {
        return dict1[value];
    }

    public A Get(B value) {
        return dict2[value];
    }

    public bool Contains(A value) {
        return dict1.ContainsKey(value);
    }

    public bool Contains(B value) {
        return dict2.ContainsKey(value);
    }

    public IEnumerator<KeyValuePair<A, B>> GetEnumerator() {
        return dict1.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return dict1.GetEnumerator();
    }
}
