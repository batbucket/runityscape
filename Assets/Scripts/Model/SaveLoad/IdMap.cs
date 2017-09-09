using Scripts.Model.Initable;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Scripts.Model.SaveLoad {

    /// <summary>
    /// Mapping of Types with strings so that we can call the constructor of derived classes
    /// when we load a save.
    /// </summary>
    /// <typeparam name="T">Type being mapped to a string.</typeparam>
    public abstract class IdMap<T> : IEnumerable<KeyValuePair<T, string>>, IInitable {
        protected Map<T, string> map;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdMap{T}"/> class.
        /// </summary>
        public IdMap() {
            this.map = new Map<T, string>();
            IdTable.AddInit(this);
        }

        /// <summary>
        /// Gets the specified t.
        /// </summary>
        /// <param name="idType">Get the string mapped with a type.</param>
        /// <returns></returns>
        public string Get(T idType) {
            return map.Get(idType);
        }

        /// <summary>
        /// Gets the specified identifier string's mapped type.
        /// </summary>
        /// <param name="idString">The identifier string.</param>
        /// <returns></returns>
        public T Get(string idString) {
            return map.Get(idString);
        }

        /// <summary>
        /// Use reflection to ensure that everything in the category is included.
        /// </summary>
        /// <returns>True if all elements are included in the map.</returns>
        public abstract bool IsAllIncluded();

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected void Init() {
            Util.Log("Starting init for " + typeof(T).ToString());
            InitHelper();
            Util.Log(string.Format("{0} initialized with {1} elements.", typeof(T).ToString(), map.ACollection.Count));
        }

        protected void Add(T t, string id) {
            map.Add(t, id);
        }

        protected abstract void InitHelper();

        IEnumerator<KeyValuePair<T, string>> IEnumerable<KeyValuePair<T, string>>.GetEnumerator() {
            return map.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return map.GetEnumerator();
        }

        void IInitable.Init() {
            this.Init();
        }
    }
}