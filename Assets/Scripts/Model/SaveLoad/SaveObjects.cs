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
using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Game.Serialized;
using Scripts.Game.Pages;
using Scripts.Game.Dungeons;

namespace Scripts.Model.SaveLoad.SaveObjects {

    [Serializable]
    public class IdSaveObject<T> {
        public string ClassId;

        public IdSaveObject(Type type) {
            this.ClassId = IdTable.Types.Get(type);
        }

        public T CreateObjectFromID() {
            Type type = IdTable.Types.Get(ClassId);
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
        public StatTypeSave(StatType st) : base(IdTable.Stats.Get(st)) { }

        public override StatType Restore() {
            return IdTable.Stats.Get(Id);
        }
    }

    [Serializable]
    public class EquipTypeSave : TypeSafeEnumSave<EquipType> {
        public EquipTypeSave(EquipType type) : base(IdTable.Equips.Get(type)) { }

        public override EquipType Restore() {
            return IdTable.Equips.Get(Id);
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
    public sealed class CharacterStatsSave : IEnumerable<StatSave> {
        public List<StatSave> Stats;
        public int Level;
        public int StatBonusCount;
        public int ResourceVisibility;

        public CharacterStatsSave(int resourceVisibility, int level, int statBonusCount, List<StatSave> stats) {
            this.ResourceVisibility = resourceVisibility;
            this.Stats = stats;
            this.Level = level;
            this.StatBonusCount = statBonusCount;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Stats.GetEnumerator();
        }

        IEnumerator<StatSave> IEnumerable<StatSave>.GetEnumerator() {
            return Stats.GetEnumerator();
        }
    }

    [Serializable]
    public sealed class SpellBookSave : IdSaveObject<SpellBook> {
        public SpellBookSave(Type type) : base(type) { }
    }

    [Serializable]
    public sealed class CharacterSpellBooksSave : IEnumerable<SpellBookSave> {
        public List<SpellBookSave> Books;

        public CharacterSpellBooksSave(List<SpellBookSave> books) {
            this.Books = books;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Books.GetEnumerator();
        }

        IEnumerator<SpellBookSave> IEnumerable<SpellBookSave>.GetEnumerator() {
            return Books.GetEnumerator();
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
    public sealed class InventorySave : IEnumerable<InventorySave.ItemCount> {
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

        IEnumerator IEnumerable.GetEnumerator() {
            return Items.GetEnumerator();
        }

        public IEnumerator<ItemCount> GetEnumerator() {
            return Items.GetEnumerator();
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
    public class EquipmentSave : IEnumerable<EquipItemSave>, IEnumerable<EquipmentSave.EquipBonus> {
        [Serializable]
        public struct EquipBonus {
            public StatTypeSave Stat;
            public int Bonus;
        }
        [Serializable]
        public class EquipBuff {
            public EquipTypeSave EquipType;
            public BuffSave Buff;
        }

        public List<EquipItemSave> Equipped;
        public List<EquipBuff> Buffs;
        public List<EquipBonus> Bonuses;

        public EquipmentSave(List<EquipItemSave> equipped, List<EquipBuff> buffs, List<EquipBonus> bonuses) {
            this.Equipped = equipped;
            this.Buffs = buffs;
            this.Bonuses = bonuses;
        }

        IEnumerator<EquipItemSave> IEnumerable<EquipItemSave>.GetEnumerator() {
            return Equipped.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Equipped.GetEnumerator();
        }

        IEnumerator<EquipBonus> IEnumerable<EquipBonus>.GetEnumerator() {
            return Bonuses.GetEnumerator();
        }
    }

    [Serializable]
    public class BuffSave : IdSaveObject<Buff> {
        public enum SaveType {
            UNKNOWN = 0,
            CASTER_IN_PARTY,
            CASTER_NOT_IN_PARTY
        }

        public SaveType Type;
        public int TurnsRemaining;
        public CharacterStatsSave StatCopy;
        public int CasterCharacterId;

        public int PartyIndex; // Setup in Party

        public BuffSave(int turnsRemaining, CharacterStatsSave statCopy, int casterId, Type type) : base(type) {
            this.TurnsRemaining = turnsRemaining;
            this.StatCopy = statCopy;
            this.CasterCharacterId = casterId;
            this.PartyIndex = -1;
        }

        public void SetupAsCasterInParty(int partyIndex) {
            this.Type = SaveType.CASTER_IN_PARTY;
            this.PartyIndex = partyIndex;
            this.StatCopy = null;
            this.CasterCharacterId = Character.UNKNOWN_ID;
        }

        public void SetupAsCasterNotInParty() {
            this.Type = SaveType.CASTER_NOT_IN_PARTY;
            this.PartyIndex = -1;
            this.CasterCharacterId = Character.UNKNOWN_ID;
        }
    }

    /// <typeparam name="T">Type in list.</typeparam>
    [Serializable]
    public class CharacterBuffsSave : IEnumerable<BuffSave> {
        public List<BuffSave> BuffSaves;

        public CharacterBuffsSave(List<BuffSave> buffSaves) {
            this.BuffSaves = buffSaves;
        }

        public static Buff SetupBuffCasterFromSave(BuffSave bs, List<Character> partyMembers) {
            Buff b = bs.CreateObjectFromID();
            b.InitFromSaveObject(bs);

            Characters.Stats caster = null;
            int id = 0;
            switch (bs.Type) {
                case BuffSave.SaveType.CASTER_IN_PARTY:
                    Util.Assert(bs.PartyIndex >= 0, "Party index is negative.");
                    Character character = partyMembers[bs.PartyIndex];
                    caster = character.Stats;
                    id = character.Id;
                    break;
                case BuffSave.SaveType.CASTER_NOT_IN_PARTY:
                    Util.Assert(bs.StatCopy != null, "Statcopy is null.");
                    caster = new Characters.Stats();
                    caster.InitFromSaveObject(bs.StatCopy);
                    id = Character.UNKNOWN_ID;
                    break;
                default:
                    Util.Assert(false, "Unknown SaveType");
                    break;
            }
            b.Caster = new BuffParams(caster, id);

            return b;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return BuffSaves.GetEnumerator();
        }

        IEnumerator<BuffSave> IEnumerable<BuffSave>.GetEnumerator() {
            return BuffSaves.GetEnumerator();
        }
    }

    /// <typeparam name="T">Type of BuffsSave used</typeparam>
    [Serializable]
    public class CharacterSave {
        public int Id;
        public CharacterStatsSave Stats;
        public CharacterBuffsSave Buffs;
        public LookSave Look;
        public CharacterSpellBooksSave Spells;
        public BrainSave Brain;
        // Party shares an inventory, so we associate it with the party
        public EquipmentSave Equipment;
        public List<Characters.Flag> Flags;

        public CharacterSave(
            int id,
            List<Characters.Flag> flags,
            CharacterStatsSave stats,
            CharacterBuffsSave buffs,
            LookSave look,
            CharacterSpellBooksSave spells,
            BrainSave brain,
            EquipmentSave equipment) {
            this.Id = id;
            this.Flags = flags;
            this.Stats = stats;
            this.Buffs = buffs;
            this.Look = look;
            this.Spells = spells;
            this.Brain = brain;
            this.Equipment = equipment;
        }
    }

    [Serializable]
    public sealed class PartySave {
        public List<CharacterSave> Characters;
        public InventorySave Inventory;

        public PartySave(List<CharacterSave> characters, InventorySave inventory) {
            this.Characters = characters;
            this.Inventory = inventory;
        }
    }

    [Serializable]
    public sealed class FlagsSave {
        public Flags Flags;

        public FlagsSave(Flags flags) {
            this.Flags = flags;
        }
    }

    [Serializable]
    public sealed class WorldSave {
        public FlagsSave Flags;
        public PartySave Party;

        public WorldSave(PartySave party, FlagsSave flags) {
            this.Party = party;
            this.Flags = flags;
        }
    }
}