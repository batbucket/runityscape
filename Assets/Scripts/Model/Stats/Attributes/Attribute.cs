using UnityEngine;

namespace Scripts.Model.Stats.Attributes {

    /// <summary>
    /// This class represents a certain stat
    /// Think strength, agility, intelligence
    /// These values have a lesser and greater cap
    /// </summary>
    public abstract class Attribute : PairedValue {
        public const int GREATER_CAP = 999;
        public const int LESSER_CAP = 1;
        public readonly AttributeType Type;

        public Attribute(int initial, AttributeType type) : base(initial) {
            this.Type = type;
        }

        public Attribute(AttributeType type) : base(Attribute.LESSER_CAP) {
            this.Type = type;
        }

        public override sealed float False {
            get {
                return Mathf.Clamp(base.False, LESSER_CAP, GREATER_CAP);
            }
            set {
                base.False = value;
            }
        }

        public override sealed int True {
            get {
                return Mathf.Clamp(base.True, LESSER_CAP, GREATER_CAP);
            }
            set {
                base.True = value;
            }
        }
    }
}