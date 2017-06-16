using Scripts.Model.Spells;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Items {

    public abstract class Item {
        public static readonly HashSet<Flag> STANDARD_FLAGS = new HashSet<Flag>() { Flag.SELLABLE, Flag.TRASHABLE };

        public readonly string Name;
        public readonly Sprite Icon;
        public readonly string Flavor;
        public readonly TargetType Target;
        public readonly int BasePrice;

        protected readonly HashSet<Flag> flags;

        private readonly int maxCount;

        private int currentCount;

        public Item(Sprite icon, int basePrice, int count, int maxCount, TargetType target, string name, string description) {
            this.flags = new HashSet<Flag>();
            foreach (Flag f in STANDARD_FLAGS) {
                flags.Add(f);
            }
            this.BasePrice = basePrice;
            this.currentCount = count;
            this.maxCount = maxCount;
            this.Target = target;
            this.Name = name;
            this.Icon = icon;
            this.Flavor = description;
            Util.Assert(currentCount <= maxCount, "Current exceeds max count.");
        }

        public int MaxCount {
            get {
                return maxCount;
            }
        }

        public int RemainingSpace {
            get {
                return MaxCount - Count;
            }
        }

        public int Count {
            get {
                return currentCount;
            }
            set {
                Util.Assert(0 <= value && value <= maxCount,
                    string.Format("Value moves {0}'s count to an invalid range. (currentCount={1}, MaxCount={2}, Input={3})", this.Name, currentCount, maxCount, value));
                currentCount = value;
            }
        }

        public bool IsFull {
            get {
                return currentCount >= maxCount;
            }
        }

        public bool HasFlag(Flag f) {
            return flags.Contains(f);
        }

        public string Description {
            get {
                string other = DescriptionHelper;
                string flavor = string.Empty;
                if (string.IsNullOrEmpty(other)) {
                    flavor = string.Format("{0}", Flavor);
                } else {
                    flavor = string.Format(flavor);
                }
                return string.Format("{0}{1}", other, flavor);
            }
        }

        protected abstract string DescriptionHelper {
            get;
        }

        public void Add(Item amount) { // 1, 1
            Util.Assert(this.Equals(amount), "Item types do not match.");

            // Extra space
            if (amount.Count > RemainingSpace) {
                Util.Log(string.Format("Extra space detected: {0} > {1}", amount.Count, RemainingSpace));
                amount.Count -= RemainingSpace;
                this.Count = MaxCount;
            } else { // Stack just fine
                amount.Count = 0;
                this.Count += amount.Count;
            }
        }

        public void Remove(Item amount) {
            Util.Assert(this.Equals(amount), "Item types do not match.");

            // Remove from one item
            if (amount.Count <= this.Count) {
                this.Count -= amount.Count;
                amount.Count = 0;
            } else { // Need to remove from multiple items
                amount.Count -= this.Count;
                this.Count = 0;
            }
        }

        public bool IsUsable(SpellParams caster, SpellParams target) {
            return true; // currentCount > 0 && IsMeetOtherRequirements(caster, target);
        }

        public override bool Equals(object obj) {
            return GetType().Equals(obj.GetType());
        }

        public override int GetHashCode() {
            return Name.GetHashCode() ^ Description.GetHashCode();
        }

        protected abstract bool IsMeetOtherRequirements(SpellParams caster, SpellParams target);


    }
}