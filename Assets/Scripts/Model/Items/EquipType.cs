using Scripts.Model.Interfaces;
using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Scripts.Model.Items {

    public sealed class EquipType : IComparable<EquipType>, INameable, ISaveable<EquipTypeSave> {
        private static readonly HashSet<EquipType> allTypes = new HashSet<EquipType>(new IdentityEqualityComparer<EquipType>());

        public static readonly EquipType WEAPON = new EquipType("Weapon", "gladius");
        public static readonly EquipType OFFHAND = new EquipType("Offhand", "round-shield");
        public static readonly EquipType ARMOR = new EquipType("Armor", "shoulder-armor");
        public static readonly EquipType TRINKET = new EquipType("Trinket", "gem-necklace");

        private static int idCounter;

        public readonly string Name;
        public readonly Sprite Sprite;
        private int id;

        private EquipType(string name, string iconLoc) {
            this.Name = name;
            this.Sprite = Util.GetSprite(iconLoc);
            this.id = idCounter++;
            allTypes.Add(this);
        }

        public static ICollection<EquipType> AllTypes {
            get {
                return new ReadOnlyCollection<EquipType>(allTypes.ToArray());
            }
        }

        string INameable.Name {
            get {
                return Name;
            }
        }

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

        public EquipTypeSave GetSaveObject() {
            return new EquipTypeSave(this);
        }

        public void InitFromSaveObject(EquipTypeSave saveObject) {
            // Nothing
        }
    }
}