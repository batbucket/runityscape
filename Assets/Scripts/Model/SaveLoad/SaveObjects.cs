using Scripts.Game.Defined.Characters;
using Scripts.Model.Characters;
using Scripts.Model.Items;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using System;
using UnityEngine;

namespace Scripts.Model.SaveLoad.SaveObjects {
    [Serializable]
    public class IdSaveObject<T> {
        private string classID;

        public IdSaveObject(Type type) {
            this.classID = IDs.Types.Get(type);
        }

        public T ObjectFromID() {
            Type type = IDs.Types.Get(classID);
            return Util.TypeToObject<T>(type);
        }
    }

    [Serializable]
    public sealed class StatSave : IdSaveObject<Stat> {

        public int Mod;
        public int Max;

        public StatSave(Type type, int mod, int max) : base(type) {
            this.Mod = mod;
            this.Max = max;
        }
    }

    [Serializable]
    public sealed class CharacterStatsSave {
        [Serializable]
        public struct EquipBonus {
            public string Stat;
            public int Bonus;
        }

        public StatSave[] Stats;

        public CharacterStatsSave(StatSave[] stats) {
            this.Stats = stats;
        }
    }

    [Serializable]
    public sealed class SpellbookSave : IdSaveObject<SpellBook> {
        public SpellbookSave(Type type) : base(type) { }
    }

    [Serializable]
    public sealed class CharacterSpellBooksSave {
        public SpellbookSave[] Books;

        public CharacterSpellBooksSave(SpellbookSave[] books) {
            this.Books = books;
        }
    }

    [Serializable]
    public sealed class BrainSave : IdSaveObject<Brain> {
        public BrainSave(Type type) : base(type) { }
    }

    [Serializable]
    public sealed class ItemSave : IdSaveObject<Item> {
        public ItemSave(Type type) : base(type) { }
    }

    [Serializable]
    public sealed class InventorySave {
        [Serializable]
        public struct ItemCount {
            public ItemSave Item;
            public int Count;
        }

        public int InventoryCapacity;
        public ItemCount[] Items;

        public InventorySave(
            int inventoryCapacity,
            ItemCount[] items) {

            this.InventoryCapacity = inventoryCapacity;
            this.Items = items;
        }
    }

    [Serializable]
    public sealed class LookSave {
        public string Name;
        public string SpriteLoc;
        public Color TextColor;
        public string Check;
        public string Tooltip;
        public Breed Breed;

        public LookSave(string name, string spriteLoc, Color text, string check, string tooltip, Breed breed) {
            this.Name = name;
            this.SpriteLoc = spriteLoc;
            this.TextColor = text;
            this.Check = check;
            this.Tooltip = tooltip;
            this.Breed = breed;
        }
    }

    [Serializable]
    public sealed class EquippableItemSave {

    }
}