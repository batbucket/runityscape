using Scripts.Model.Characters;
using UnityEngine;

namespace Scripts.Model.Stats {

    /// <summary>
    /// This class represents a certain stat
    /// Think strength, agility, intelligence
    /// These values have a lesser and greater cap
    /// </summary>
    public abstract class Stat {

        public readonly StatType Type;

        private float mod;
        private int max;

        public Stat(float mod, int max, StatType type) {
            this.mod = mod;
            this.max = max;
            this.Type = type;
        }

        public float Mod {
            get {
                return mod;
            }
            set {
                SetMod(value, true);
            }
        }

        public void SetMod(float amount, bool isObeyMaxCap) {
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

        /// <summary>
        /// Update the values of this Stat
        /// </summary>
        public virtual void Update(Character c) { }
    }
}