using System;

namespace Scripts.Model.Items {

    public sealed class EquipType : IComparable<EquipType> {
        private static int idCounter;

        public readonly string Name;
        private int id;

        private EquipType(string name) {
            this.Name = name;
            this.id = idCounter++;
        }

        public static readonly EquipType WEAPON = new EquipType("Weapon");
        public static readonly EquipType OFFHAND = new EquipType("Offhand");
        public static readonly EquipType ARMOR = new EquipType("Armor");
        public static readonly EquipType TRINKET = new EquipType("Trinket");

        public override bool Equals(object obj) {
            return this == obj;
        }

        public override int GetHashCode() {
            return id;
        }

        public override string ToString() {
            return this.Name;
        }

        int IComparable<EquipType>.CompareTo(EquipType other) {
            return this.Name.CompareTo(other.Name);
        }
    }
}