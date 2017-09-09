using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue> {
    /// <summary>
    ///
    /// </summary>
    internal class KeyCollection : ICollection<TKey> {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyCollection"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <exception cref="System.ArgumentNullException">collection</exception>
        public KeyCollection(ICollection<TKey> collection) {
            if (collection == null) { throw new ArgumentNullException("collection"); }

            _collection = collection;
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TKey> GetEnumerator() {
            return _collection.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="System.NotSupportedException">Collection is read-only.</exception>
        public void Add(TKey item) {
            throw new NotSupportedException("Collection is read-only.");
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        /// <exception cref="System.NotSupportedException">Collection is read-only.</exception>
        public void Clear() {
            throw new NotSupportedException("Collection is read-only.");
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(TKey item) {
            return _collection.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(TKey[] array, int arrayIndex) {
            _collection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">Collection is read-only.</exception>
        public bool Remove(TKey item) {
            throw new NotSupportedException("Collection is read-only.");
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count {
            get { return _collection.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly {
            get { return true; }
        }

        /// <summary>
        /// The collection
        /// </summary>
        private readonly ICollection<TKey> _collection;
    }

    /// <summary>
    ///
    /// </summary>
    internal class ValueCollection : ICollection<TValue> {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueCollection"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <exception cref="System.ArgumentNullException">collection</exception>
        public ValueCollection(ICollection<TValue> collection) {
            if (collection == null) { throw new ArgumentNullException("collection"); }

            _collection = collection;
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TValue> GetEnumerator() {
            return _collection.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="System.NotSupportedException">Collection is read-only.</exception>
        public void Add(TValue item) {
            throw new NotSupportedException("Collection is read-only.");
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        /// <exception cref="System.NotSupportedException">Collection is read-only.</exception>
        public void Clear() {
            throw new NotSupportedException("Collection is read-only.");
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(TValue item) {
            return _collection.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(TValue[] array, int arrayIndex) {
            _collection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">Collection is read-only.</exception>
        public bool Remove(TValue item) {
            throw new NotSupportedException("Collection is read-only.");
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count {
            get { return _collection.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly {
            get { return true; }
        }

        /// <summary>
        /// The collection
        /// </summary>
        private readonly ICollection<TValue> _collection;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadOnlyDictionary{TKey, TValue}"/> class.
    /// </summary>
    /// <param name="dictionary">The dictionary.</param>
    public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary) {
        _dictionary = dictionary;
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns></returns>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
        return _dictionary.GetEnumerator();
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    /// <summary>
    /// Adds the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <exception cref="System.NotSupportedException">Collection is read-only.</exception>
    public void Add(KeyValuePair<TKey, TValue> item) {
        throw new NotSupportedException("Collection is read-only.");
    }

    /// <summary>
    /// Clears this instance.
    /// </summary>
    /// <exception cref="System.NotSupportedException">Collection is read-only.</exception>
    public void Clear() {
        throw new NotSupportedException("Collection is read-only.");
    }

    /// <summary>
    /// Determines whether [contains] [the specified item].
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>
    ///   <c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(KeyValuePair<TKey, TValue> item) {
        return _dictionary.Contains(item);
    }

    /// <summary>
    /// Copies to.
    /// </summary>
    /// <param name="array">The array.</param>
    /// <param name="arrayIndex">Index of the array.</param>
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
        _dictionary.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Removes the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns></returns>
    /// <exception cref="System.NotSupportedException">Collection is read-only.</exception>
    public bool Remove(KeyValuePair<TKey, TValue> item) {
        throw new NotSupportedException("Collection is read-only.");
    }

    /// <summary>
    /// Gets the count.
    /// </summary>
    /// <value>
    /// The count.
    /// </value>
    public int Count {
        get { return _dictionary.Count; }
    }

    /// <summary>
    /// Gets a value indicating whether this instance is read only.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is read only; otherwise, <c>false</c>.
    /// </value>
    public bool IsReadOnly {
        get { return true; }
    }

    /// <summary>
    /// Determines whether the specified key contains key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>
    ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
    /// </returns>
    public bool ContainsKey(TKey key) {
        return _dictionary.ContainsKey(key);
    }

    /// <summary>
    /// Adds the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <exception cref="System.NotSupportedException">Collection is read-only.</exception>
    public void Add(TKey key, TValue value) {
        throw new NotSupportedException("Collection is read-only.");
    }

    /// <summary>
    /// Removes the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    /// <exception cref="System.NotSupportedException">Collection is read-only.</exception>
    public bool Remove(TKey key) {
        throw new NotSupportedException("Collection is read-only.");
    }

    /// <summary>
    /// Tries the get value.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public bool TryGetValue(TKey key, out TValue value) {
        return _dictionary.TryGetValue(key, out value);
    }

    /// <summary>
    /// Gets or sets the <see cref="TValue"/> with the specified key.
    /// </summary>
    /// <value>
    /// The <see cref="TValue"/>.
    /// </value>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    /// <exception cref="System.NotSupportedException">Collection is read-only.</exception>
    public TValue this[TKey key] {
        get { return _dictionary[key]; }
        set { throw new NotSupportedException("Collection is read-only."); }
    }

    /// <summary>
    /// Gets the keys.
    /// </summary>
    /// <value>
    /// The keys.
    /// </value>
    public ICollection<TKey> Keys {
        get { return _keys ?? (_keys = new KeyCollection(_dictionary.Keys)); }
    }

    /// <summary>
    /// Gets the values.
    /// </summary>
    /// <value>
    /// The values.
    /// </value>
    public ICollection<TValue> Values {
        get { return _values ?? (_values = new ValueCollection(_dictionary.Values)); }
    }

    /// <summary>
    /// The keys
    /// </summary>
    private KeyCollection _keys;
    /// <summary>
    /// The values
    /// </summary>
    private ValueCollection _values;
    /// <summary>
    /// The dictionary
    /// </summary>
    private readonly IDictionary<TKey, TValue> _dictionary;
}