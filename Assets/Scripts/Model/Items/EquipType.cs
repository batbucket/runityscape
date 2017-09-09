using Scripts.Model.Interfaces;
using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Scripts.Model.Items {

    /// <summary>
    /// This type is used to denote what part of the body a piece of equipment is equipped in.
    /// </summary>
    /// <seealso cref="System.IComparable{Scripts.Model.Items.EquipType}" />
    /// <seealso cref="Scripts.Model.Interfaces.INameable" />
    /// <seealso cref="Scripts.Model.SaveLoad.ISaveable{Scripts.Model.SaveLoad.SaveObjects.EquipTypeSave}" />
    public sealed class EquipType : IComparable<EquipType>, INameable, ISaveable<EquipTypeSave> {
        private static readonly HashSet<EquipType> allTypes = new HashSet<EquipType>(new IdentityEqualityComparer<EquipType>());

        public static readonly EquipType WEAPON = new EquipType("Weapon", "gladius");
        public static readonly EquipType OFFHAND = new EquipType("Offhand", "round-shield");
        public static readonly EquipType ARMOR = new EquipType("Armor", "shoulder-armor");
        public static readonly EquipType TRINKET = new EquipType("Trinket", "gem-necklace");

        private static int idCounter;

        /// <summary>
        /// The name of the equipment slot
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// The sprite of the equipment slot
        /// </summary>
        public readonly Sprite Sprite;

        private int id;

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipType"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="iconLoc">The icon string location.</param>
        private EquipType(string name, string iconLoc) {
            this.Name = name;
            this.Sprite = Util.GetSprite(iconLoc);
            this.id = idCounter++;
            allTypes.Add(this);
        }

        /// <summary>
        /// Gets all types of the type safe enum.
        /// </summary>
        /// <value>
        /// All types.
        /// </value>
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

        /// <summary>
        /// Gets the save object.
        /// </summary>
        /// <returns>A save object.</returns>
        public EquipTypeSave GetSaveObject() {
            return new EquipTypeSave(this);
        }

        /// <summary>
        /// Initializes from save object.
        /// </summary>
        /// <param name="saveObject">The save object.</param>
        public void InitFromSaveObject(EquipTypeSave saveObject) {
            // Nothing
        }
    }
}