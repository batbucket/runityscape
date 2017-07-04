using System;
using System.Collections;
using System.Collections.Generic;

public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue> {
    internal class KeyCollection : ICollection<TKey> {
        public KeyCollection(ICollection<TKey> collection) {
            if (collection == null) { throw new ArgumentNullException("collection"); }

            _collection = collection;
        }

        public IEnumerator<TKey> GetEnumerator() {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(TKey item) {
            throw new NotSupportedException("Collection is read-only.");
        }

        public void Clear() {
            throw new NotSupportedException("Collection is read-only.");
        }

        public bool Contains(TKey item) {
            return _collection.Contains(item);
        }

        public void CopyTo(TKey[] array, int arrayIndex) {
            _collection.CopyTo(array, arrayIndex);
        }

        public bool Remove(TKey item) {
            throw new NotSupportedException("Collection is read-only.");
        }

        public int Count {
            get { return _collection.Count; }
        }

        public bool IsReadOnly {
            get { return true; }
        }

        private readonly ICollection<TKey> _collection;
    }

    internal class ValueCollection : ICollection<TValue> {
        public ValueCollection(ICollection<TValue> collection) {
            if (collection == null) { throw new ArgumentNullException("collection"); }

            _collection = collection;
        }

        public IEnumerator<TValue> GetEnumerator() {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(TValue item) {
            throw new NotSupportedException("Collection is read-only.");
        }

        public void Clear() {
            throw new NotSupportedException("Collection is read-only.");
        }

        public bool Contains(TValue item) {
            return _collection.Contains(item);
        }

        public void CopyTo(TValue[] array, int arrayIndex) {
            _collection.CopyTo(array, arrayIndex);
        }

        public bool Remove(TValue item) {
            throw new NotSupportedException("Collection is read-only.");
        }

        public int Count {
            get { return _collection.Count; }
        }

        public bool IsReadOnly {
            get { return true; }
        }

        private readonly ICollection<TValue> _collection;
    }

    public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary) {
        _dictionary = dictionary;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
        return _dictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    public void Add(KeyValuePair<TKey, TValue> item) {
        throw new NotSupportedException("Collection is read-only.");
    }

    public void Clear() {
        throw new NotSupportedException("Collection is read-only.");
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) {
        return _dictionary.Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
        _dictionary.CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item) {
        throw new NotSupportedException("Collection is read-only.");
    }

    public int Count {
        get { return _dictionary.Count; }
    }

    public bool IsReadOnly {
        get { return true; }
    }

    public bool ContainsKey(TKey key) {
        return _dictionary.ContainsKey(key);
    }

    public void Add(TKey key, TValue value) {
        throw new NotSupportedException("Collection is read-only.");
    }

    public bool Remove(TKey key) {
        throw new NotSupportedException("Collection is read-only.");
    }

    public bool TryGetValue(TKey key, out TValue value) {
        return _dictionary.TryGetValue(key, out value);
    }

    public TValue this[TKey key] {
        get { return _dictionary[key]; }
        set { throw new NotSupportedException("Collection is read-only."); }
    }

    public ICollection<TKey> Keys {
        get { return _keys ?? (_keys = new KeyCollection(_dictionary.Keys)); }
    }

    public ICollection<TValue> Values {
        get { return _values ?? (_values = new ValueCollection(_dictionary.Values)); }
    }

    private KeyCollection _keys;
    private ValueCollection _values;
    private readonly IDictionary<TKey, TValue> _dictionary;
}