using Scripts.Model.Characters;
using Scripts.Model.SaveLoad.SaveObjects;
using UnityEngine;
using Scripts.Model.SaveLoad;

namespace Scripts.Model.Stats {

    /// <summary>
    /// This class represents a certain stat
    /// Think strength, agility, intelligence
    /// These values have a lesser and greater cap
    /// </summary>
    /// <seealso cref="Scripts.Model.SaveLoad.ISaveable{Scripts.Model.SaveLoad.SaveObjects.StatSave}" />
    public abstract class Stat : ISaveable<StatSave> {

        /// <summary>
        /// The type
        /// </summary>
        public readonly StatType Type;

        /// <summary>
        /// The modified value
        /// </summary>
        private int mod;

        /// <summary>
        /// The maximum value, serves as a reference point for stat recovery.
        /// </summary>
        private int max;

        /// <summary>
        /// Initializes a new instance of the <see cref="Stat"/> class.
        /// </summary>
        /// <param name="mod">The mod.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="type">The type.</param>
        public Stat(int mod, int max, StatType type) {
            this.mod = mod;
            this.max = max;
            this.Type = type;
        }

        /// <summary>
        /// Gets or sets the mod.
        /// </summary>
        /// <value>
        /// The mod.
        /// </value>
        public int Mod {
            get {
                return mod;
            }
            set {
                SetMod(value, true);
            }
        }

        /// <summary>
        /// Sets the mod.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="isObeyMaxCap">if set to <c>true</c> [is obey maximum cap].</param>
        public void SetMod(int amount, bool isObeyMaxCap) {
            if (isObeyMaxCap) {
                mod = Mathf.Clamp(amount, Type.Bounds.Low, Max);
            } else {
                mod = Mathf.Clamp(amount, Type.Bounds.Low, Type.Bounds.High);
            }
        }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>
        /// The maximum.
        /// </value>
        public int Max {
            get {
                return max;
            }
            set {
                max = Mathf.Clamp(value, Type.Bounds.Low, Type.Bounds.High);
                mod = Mathf.Clamp(mod, Type.Bounds.Low, max);
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            var item = obj as Stat;

            if (item == null) {
                return false;
            }

            return this.GetType().Equals(item.GetType())
                && this.Type.Equals(item.Type)
                && this.mod.Equals(item.mod)
                && this.max.Equals(item.max);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode() {
            return Type.GetHashCode();
        }

        /// <summary>
        /// Update the values of this Stat
        /// </summary>
        /// <param name="c">The c.</param>
        public virtual void Update(Character c) { }

        /// <summary>
        /// Gets the save object. A save object contains the neccessary
        /// information to initialize a clean class to its saved state.
        /// A save object is also serializable.
        /// </summary>
        /// <returns></returns>
        public StatSave GetSaveObject() {
            StatSave save = new StatSave(Type.GetSaveObject(), GetType(), mod, max);
            return save;
        }

        /// <summary>
        /// Initializes from save object.
        /// </summary>
        /// <param name="saveObject">The save object.</param>
        public void InitFromSaveObject(StatSave saveObject) {
            this.mod = saveObject.Mod;
            this.max = saveObject.Max;
        }
    }
}