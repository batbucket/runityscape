using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mapping that guarentees unique A will be mapped with unique B.
/// </summary>
/// <typeparam name="A"></typeparam>
/// <typeparam name="B"></typeparam>
public class Map<A, B> : IEnumerable<KeyValuePair<A, B>> {
    private IDictionary<A, B> dict1;
    private IDictionary<B, A> dict2;

    /// <summary>
    /// Initializes a new instance of the <see cref="Map{A, B}"/> class.
    /// </summary>
    public Map() {
        this.dict1 = new Dictionary<A, B>();
        this.dict2 = new Dictionary<B, A>();
    }

    /// <summary>
    /// Gets the a collection.
    /// </summary>
    /// <value>
    /// a collection.
    /// </value>
    public ICollection<A> ACollection {
        get {
            return dict1.Keys;
        }
    }

    /// <summary>
    /// Gets the b collection.
    /// </summary>
    /// <value>
    /// The b collection.
    /// </value>
    public ICollection<B> BCollection {
        get {
            return dict2.Keys;
        }
    }

    /// <summary>
    /// Adds the specified key1 associated with key2.
    /// </summary>
    /// <param name="key1">The key1.</param>
    /// <param name="key2">The key2.</param>
    public void Add(A key1, B key2) {
        Util.Assert(!Contains(key1) && !Contains(key2), string.Format("Duplicate elements detected: <{0}, {1}>", key1.ToString(), key2.ToString()));
        dict1.Add(key1, key2);
        dict2.Add(key2, key1);
    }

    /// <summary>
    /// Gets the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public B Get(A value) {
        return dict1[value];
    }

    /// <summary>
    /// Gets the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public A Get(B value) {
        return dict2[value];
    }

    /// <summary>
    /// Determines whether [contains] [the specified value].
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    ///   <c>true</c> if [contains] [the specified value]; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(A value) {
        return dict1.ContainsKey(value);
    }

    /// <summary>
    /// Determines whether map contains the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    ///   <c>true</c> if map contains the specified value; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(B value) {
        return dict2.ContainsKey(value);
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns></returns>
    public IEnumerator<KeyValuePair<A, B>> GetEnumerator() {
        return dict1.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return dict1.GetEnumerator();
    }
}
