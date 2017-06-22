using Scripts.Game.Defined.Characters;
using Scripts.Model.Buffs;
using Scripts.Model.Characters;
using Scripts.Model.Items;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

namespace Scripts.Model.SaveLoad.SaveObjects {

    [Serializable]
    public class IdSaveObject<T> {
        public string ClassId;

        public IdSaveObject(Type type) {
            this.ClassId = IDs.Types.Get(type);
        }

        public T CreateObjectFromID() {
            Type type = IDs.Types.Get(ClassId);
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

    // Needed so we can put a constraint on BuffSave, which has a generic param
    public interface IBuffSave { }

    [Serializable]
    public abstract class BuffSave : IdSaveObject<Buff>, IBuffSave {
        public int TurnsRemaining;

        public BuffSave(Type type, int turnsRemaining) : base(type) {
            this.TurnsRemaining = turnsRemaining;
        }
    }

    // Not serializable
    public sealed class PartialBuffSave : BuffSave {
        public Type Type;
        public CharacterStatsSave StatCopy;
        public int CasterCharacterId;

        public PartialBuffSave(int turnsRemaining, CharacterStatsSave casterCopy, int casterId, Type type) : base(type, turnsRemaining) {
            this.StatCopy = casterCopy;
            this.CasterCharacterId = casterId;
            this.Type = type;
        }
    }

    [Serializable]
    public sealed class FullBuffSave : BuffSave {
        public const int CASTER_NOT_IN_PARTY = -1;

        public int PartyIndex;
        public CharacterStatsSave Stats;

        public FullBuffSave(Type type, int turnsRemaining, int partyIndex) : base(type, turnsRemaining) {
            this.PartyIndex = partyIndex;
            this.Stats = null;
        }

        public FullBuffSave(Type type, int turnsRemaining, CharacterStatsSave stats) : base(type, turnsRemaining) {
            this.PartyIndex = CASTER_NOT_IN_PARTY;
            this.Stats = stats;
        }

        public bool IsCasterInParty {
            get {
                return PartyIndex >= 0;
            }
        }
    }

    // Needed so we can put a constraint on CharacterBuffsSave, which has a generic param
    public interface ICharacterBuffsSave { }

    /// <typeparam name="T">Type in list.</typeparam>
    [Serializable]
    public abstract class CharacterBuffsSave<T> : IEnumerable<T>, ICharacterBuffsSave where T : BuffSave {
        public List<T> BuffSaves;

        public CharacterBuffsSave(List<T> buffSaves) {
            this.BuffSaves = buffSaves;
        }

        public IEnumerator<T> GetEnumerator() {
            return BuffSaves.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return BuffSaves.GetEnumerator();
        }
    }

    // Not serializable
    public sealed class PartialCharacterBuffsSave : CharacterBuffsSave<PartialBuffSave> {
        public PartialCharacterBuffsSave(List<PartialBuffSave> buffSaves) : base(buffSaves) { }
    }

    [Serializable]
    public sealed class FullCharacterBuffsSave : CharacterBuffsSave<FullBuffSave> {
        public FullCharacterBuffsSave(List<FullBuffSave> buffSaves) : base(buffSaves) { }
    }

    /// <typeparam name="T">Type of BuffsSave used</typeparam>
    [Serializable]
    public abstract class CharacterSave<T> where T : ICharacterBuffsSave {
        public CharacterStatsSave Stats;
        public T Buffs;
        public LookSave Look;
        public CharacterSpellBooksSave Spells;
        public BrainSave Brain;
        // Party shares an inventory, so we associate it with the party
        public EquipmentSave Equipment;

        public CharacterSave(
            CharacterStatsSave stats,
            T buffs,
            LookSave look,
            CharacterSpellBooksSave spells,
            BrainSave brain,
            EquipmentSave equipment) {
            this.Stats = stats;
            this.Buffs = buffs;
            this.Look = look;
            this.Spells = spells;
            this.Brain = brain;
            this.Equipment = equipment;
        }
    }

    // Not serializable
    public sealed class PartialCharacterSave : CharacterSave<PartialCharacterBuffsSave> {
        public int Id;

        public PartialCharacterSave(
            int id,
            CharacterStatsSave stats,
            PartialCharacterBuffsSave buffs,
            LookSave look,
            CharacterSpellBooksSave spells,
            BrainSave brain,
            EquipmentSave equipment
            )
            : base(stats, buffs, look, spells, brain, equipment) {
            this.Id = id;
        }
    }

    [Serializable]
    public sealed class FullCharacterSave : CharacterSave<FullCharacterBuffsSave> {
        public FullCharacterSave(
            CharacterStatsSave stats,
            FullCharacterBuffsSave buffs,
            LookSave look,
            CharacterSpellBooksSave spells,
            BrainSave brain,
            EquipmentSave equipment
            )
            : base(stats, buffs, look, spells, brain, equipment) {
        }
    }

    [Serializable]
    public sealed class PartySave {
        public List<FullCharacterSave> Characters;
        public InventorySave Inventory;

        public PartySave(List<FullCharacterSave> characters, InventorySave inventory) {
            this.Characters = characters;
            this.Inventory = inventory;
        }
    }
}