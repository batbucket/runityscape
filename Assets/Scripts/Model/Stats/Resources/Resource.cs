using Scripts.Model.Stats.Attributes;
using UnityEngine;

namespace Scripts.Model.Stats.Resources {

    /// <summary>
    /// This class represents a Resource
    /// Think health, mana, etc.
    ///
    /// These values have a lesser cap of 0
    /// and a variable greater cap
    /// </summary>
    public abstract class Resource : PairedValue {
        public const int GREATER_CAP = 99999;
        public const int LESSER_CAP = 0;
        public readonly ResourceType Type;
        public AttributeType Dependent;

        private readonly bool isFalseCappedAtTrue;

        public Resource(int trueValue, int falseValue, ResourceType type, bool isFalseCappedAtTrue) : base(trueValue, falseValue) {
            this.Type = type;
            this.IsVisible = true;
            this.isFalseCappedAtTrue = isFalseCappedAtTrue;
        }

        public Resource(int initial, ResourceType type, bool isFalseCappedAtTrue) : this(initial, initial, type, isFalseCappedAtTrue) {
        }

        public override float False {
            get {
                return Mathf.Clamp(base.False, LESSER_CAP, isFalseCappedAtTrue ? True : GREATER_CAP);
            }
            set {
                base.False = value;
            }
        }

        public bool IsVisible { get; set; }

        public override int True {
            get {
                return Mathf.Clamp(base.True, LESSER_CAP, GREATER_CAP);
            }
            set {
                base.True = value;
            }
        }

        public virtual void Calculate(int attribute) {
        }
    }
}