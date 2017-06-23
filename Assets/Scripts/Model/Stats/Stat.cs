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
    public abstract class Stat : ISaveable<StatSave> {

        public readonly StatType Type;

        private int mod;
        private int max;

        public Stat(int mod, int max, StatType type) {
            this.mod = mod;
            this.max = max;
            this.Type = type;
        }

        public int Mod {
            get {
                return mod;
            }
            set {
                SetMod(value, true);
            }
        }

        public void SetMod(int amount, bool isObeyMaxCap) {
            if (isObeyMaxCap) {
                mod = Mathf.Clamp(amount, Type.Bounds.Low, Max);
            } else {
                mod = Mathf.Clamp(amount, Type.Bounds.Low, Type.Bounds.High);
            }
        }

        public int Max {
            get {
                return max;
            }
            set {
                max = Mathf.Clamp(value, Type.Bounds.Low, Type.Bounds.High);
            }
        }

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

        public override int GetHashCode() {
            return Type.GetHashCode();
        }

        /// <summary>
        /// Update the values of this Stat
        /// </summary>
        public virtual void Update(Character c) { }

        public StatSave GetSaveObject() {
            StatSave save = new StatSave(Type.GetSaveObject(), GetType(), mod, max);
            return save;
        }

        public void InitFromSaveObject(StatSave saveObject) {
            this.mod = saveObject.Mod;
            this.max = saveObject.Max;
        }
    }
}