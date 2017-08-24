using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Model.Spells;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scripts.Model.Interfaces;

namespace Scripts.Model.Items {

    public abstract class Item : ISaveable<ItemSave>, ISpellable {

        public readonly string Name;
        public readonly Sprite Icon;
        public readonly string Flavor;
        public readonly TargetType Target;
        public readonly int BasePrice;

        protected readonly HashSet<Flag> flags;

        public Item(Sprite icon, int basePrice, TargetType target, string name, string description) {
            this.flags = new HashSet<Flag>();
            this.BasePrice = basePrice;
            this.Target = target;
            this.Name = name;
            this.Icon = icon;
            this.Flavor = description;
        }

        public bool HasFlag(Flag f) {
            return flags.Contains(f);
        }

        public string Description {
            get {
                return DescriptionHelper;
            }
        }

        protected abstract string DescriptionHelper {
            get;
        }

        public bool IsUsable(SpellParams caster, SpellParams target) {
            return caster.Stats.State == Characters.State.ALIVE;
        }

        public override bool Equals(object obj) {
            Item item = obj as Item;
            if (item == null) {
                return false;
            }
            return GetType().Equals(item.GetType());
        }

        public override int GetHashCode() {
            return Name.GetHashCode() ^ Description.GetHashCode();
        }

        protected abstract bool IsMeetOtherRequirements(SpellParams caster, SpellParams target);

        public ItemSave GetSaveObject() {
            return new ItemSave(GetType());
        }

        public void InitFromSaveObject(ItemSave saveObject) {
            // Nothing
        }

        public abstract SpellBook GetSpellBook();
    }
}