using Scripts.Game.Defined.Characters;
using Scripts.Model.Buffs;
using Scripts.Model.Characters;
using Scripts.Model.Items;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using System;
using System.Collections.Generic;
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
    public abstract class TypeSafeEnumSave<T> {
        public string Id;

        public TypeSafeEnumSave(string id) {
            this.Id = id;
        }

        public abstract T Restore();
    }

    [Serializable]
    public class StatTypeSave : TypeSafeEnumSave<StatType> {
        public StatTypeSave(StatType st) : base(IDs.Stats.Get(st)) { }

        public override StatType Restore() {
            return IDs.Stats.Get(Id);
        }
    }

    [Serializable]
    public class EquipTypeSave : TypeSafeEnumSave<EquipType> {
        public EquipTypeSave(EquipType type) : base(IDs.Equips.Get(type)) { }

        public override EquipType Restore() {
            return IDs.Equips.Get(Id);
        }
    }

    [Serializable]
    public sealed class StatSave : IdSaveObject<Stat> {
        public StatTypeSave StatType;
        public int Mod;
        public int Max;

        public StatSave(StatTypeSave statType, Type type, int mod, int max) : base(type) {
            this.StatType = statType;
            this.Mod = mod;
            this.Max = max;
        }
    }

    [Serializable]
    public sealed class CharacterStatsSave {
        public List<StatSave> Stats;

        public CharacterStatsSave(List<StatSave> stats) {
            this.Stats = stats;
        }
    }

    [Serializable]
    public sealed class SpellbookSave : IdSaveObject<SpellBook> {
        public SpellbookSave(Type type) : base(type) { }
    }

    [Serializable]
    public sealed class CharacterSpellBooksSave {
        public List<SpellbookSave> Books;

        public CharacterSpellBooksSave(List<SpellbookSave> books) {
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
        public List<ItemCount> Items;

        public InventorySave(
            int inventoryCapacity,
            List<ItemCount> items) {

            this.InventoryCapacity = inventoryCapacity;
            this.Items = items;
        }
    }

    [Serializable]
    public sealed class LookSave {
        public string Name;
        public Sprite Sprite;
        public Color TextColor;
        public string Check;
        public string Tooltip;
        public Breed Breed;

        public LookSave(string name, Sprite sprite, Color text, string check, string tooltip, Breed breed) {
            this.Name = name;
            this.Sprite = sprite;
            this.TextColor = text;
            this.Check = check;
            this.Tooltip = tooltip;
            this.Breed = breed;
        }
    }

    [Serializable]
    public sealed class EquipItemSave : IdSaveObject<EquippableItem> {
        public EquipTypeSave EquipTypeSave;

        public EquipItemSave(EquipTypeSave equipTypeSave, Type type) : base(type) {
            this.EquipTypeSave = equipTypeSave;
        }
    }

    [Serializable]
    public sealed class EquipmentSave {
        [Serializable]
        public struct EquipBonus {
            public string Stat;
            public int Bonus;
        }

        public List<EquipItemSave> Equipped;
        // TODO Item buffs
        public List<EquipBonus> Bonuses;

        public EquipmentSave(List<EquipItemSave> equipped, List<EquipBonus> bonuses) {
            this.Equipped = equipped;
            this.Bonuses = bonuses;
        }
    }

    [Serializable]
    public sealed class BuffSave : IdSaveObject<Buff> {
        public enum CasterType {
            UNDEFINED,
            IN_PARTY,
            NOT_IN_PARTY,
        }

        public CasterType Type;
        public int TurnsRemaining;
        public CharacterStatsSave StatCopy;
        public int PartyIndex;

        public BuffSave(int turnsRemaining, CharacterStatsSave casterCopy, Type type) : base(type) {
            this.TurnsRemaining = turnsRemaining;
            this.StatCopy = casterCopy;

            // Assume caster is not in party during construction
            this.Type = CasterType.UNDEFINED;
            this.PartyIndex = -1;
        }
    }

    [Serializable]
    public sealed class CharacterBuffsSave {
        public List<BuffSave> BuffSaves;

        public CharacterBuffsSave(List<BuffSave> buffSaves) {
            this.BuffSaves = buffSaves;
        }
    }

    [Serializable]
    public sealed class CharacterSave {
        public CharacterStatsSave Stats;
        public CharacterBuffsSave Buffs;
        public LookSave Look;
        public CharacterSpellBooksSave Spells;
        public BrainSave Brain;
        public InventorySave Inventory;
        public EquipmentSave Equipment;

        public CharacterSave(
            CharacterStatsSave stats,
            CharacterBuffsSave buffs,
            LookSave look,
            CharacterSpellBooksSave spells,
            BrainSave brain,
            InventorySave inventory,
            EquipmentSave equipment
            ) {
            this.Stats = stats;
            this.Buffs = buffs;
            this.Look = look;
            this.Spells = spells;
            this.Brain = brain;
            this.Inventory = inventory;
            this.Equipment = equipment;
        }
    }
}