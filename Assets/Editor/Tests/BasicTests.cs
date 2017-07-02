using NUnit.Framework;
using Scripts.Game.Defined.Characters;
using Scripts.Game.Defined.Serialized.Characters;
using Scripts.Game.Defined.Serialized.Items.Consumables;
using Scripts.Game.Defined.Serialized.Items.Equipment;
using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Game.Defined.Serialized.Statistics;
using Scripts.Game.Defined.StartingStats;
using Scripts.Model.Acts;
using Scripts.Model.Buffs;
using Scripts.Model.Characters;
using Scripts.Model.Items;
using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using Scripts.Model.TextBoxes;
using Scripts.Model.World.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[TestFixture]
public class BasicTests {

    [TestFixture]
    public class IDTableTests {
        [OneTimeSetUp]
        public void Setup() {
            IdTable.Init();
        }

        [Test]
        public void TypeIDTableContainsAllSerializableMaterial() {
            IDTableIncludesEverythingHelper(IdTable.Types);
        }

        [Test]
        public void StatTypeIDTableContainsAllStatTypes() {
            IDTableIncludesEverythingHelper(IdTable.Stats);
        }

        [Test]
        public void EquipTypeIDTableContainsAllStatTypes() {
            IDTableIncludesEverythingHelper(IdTable.Equips);
        }

        private void IDTableIncludesEverythingHelper<T>(IdMap<T> idMap) {
            Assert.IsTrue(idMap.IsAllIncluded());
        }
    }

    [TestFixture]
    public class SerializationTests {
        [OneTimeSetUp]
        public void Setup() {
            IdTable.Init();
        }

        public string ToJson(object o) {
            string json = JsonUtility.ToJson(o);
            Util.Log("Serialized to:\n" + json);
            return json;
        }

        public T FromJson<T>(string json) {
            return JsonUtility.FromJson<T>(json);
        }

        [Serializable]
        public sealed class ArrayWrapper {
            public int[] Array;

            public ArrayWrapper(int[] array) {
                this.Array = array;
            }
        }

        [Serializable]
        public sealed class ListWrapper {
            public List<int> List;

            public ListWrapper(List<int> list) {
                this.List = list;
            }
        }

        [Test]
        public void ListsAreSerializable() {
            List<int> list = new List<int>();
            list.Add(0);
            list.Add(1);
            list.Add(2);
            list.Add(3);

            string json = JsonUtility.ToJson(new ListWrapper(list));

            ListWrapper restored = JsonUtility.FromJson<ListWrapper>(json);
            Assert.AreEqual(list.Count, restored.List.Count);
            for (int i = 0; i <= 3; i++) {
                Assert.AreEqual(restored.List[i], i);
            }
        }

        [Test]
        public void ArraysAreSerializable() {
            int[] list = new int[4];
            list[0] = 0;
            list[1] = 1;
            list[2] = 2;
            list[3] = 3;

            string json = JsonUtility.ToJson(new ArrayWrapper(list));

            ArrayWrapper restored = JsonUtility.FromJson<ArrayWrapper>(json);
            Assert.AreEqual(list.Length, restored.Array.Length);
            for (int i = 0; i <= 3; i++) {
                Assert.AreEqual(restored.Array[i], i);
            }
        }

        /// <typeparam name="O">Type being serialized</typeparam>
        /// <typeparam name="S">SaveObject used to save</typeparam>
        /// <param name=""></param>
        private void IdSerializableHelper<O, S>(O saveable) where S : IdSaveObject<O> where O : ISaveable<S> {
            S initialSaveObject = saveable.GetSaveObject();
            string json = ToJson(initialSaveObject);
            S newSaveObject = FromJson<S>(json);
            O newObject = newSaveObject.CreateObjectFromID();
            newObject.InitFromSaveObject(newSaveObject);
            Assert.IsTrue(saveable.Equals(newObject));
            Assert.AreEqual(saveable, newObject);
        }

        private void BasicSerializableHelper<O, S>(O saveable) where O : ISaveable<S> {
            S initialSaveObject = saveable.GetSaveObject();
            string json = ToJson(initialSaveObject);
            S newSaveObject = FromJson<S>(json);
            O newObject = Util.TypeToObject<O>(typeof(O));
            newObject.InitFromSaveObject(newSaveObject);
            Assert.IsTrue(saveable.Equals(newObject));
            Assert.AreEqual(saveable, newObject);
        }

        private void TypeSafeEnumSerializableHelper<T, S>(T type) where S : TypeSafeEnumSave<T> where T : ISaveable<S> {
            S initialSaveObject = type.GetSaveObject();
            string json = ToJson(initialSaveObject);
            S newSaveObject = FromJson<S>(json);
            ISaveable<S> newObject = newSaveObject.Restore();
            Assert.AreEqual(type, newObject);
            Assert.AreSame(type, newObject);
        }

        private void ContainerSerializationHelper<O, S, T>(O original) where O : ISaveable<S>, IEnumerable<T> {
            S initialSaveObject = original.GetSaveObject();
            string json = ToJson(initialSaveObject);
            S newSaveObject = FromJson<S>(json);
            O newObject = Util.TypeToObject<O>(original.GetType());
            newObject.InitFromSaveObject(newSaveObject);
            Assert.AreEqual(original, newObject);
            CollectionAssert.AreEquivalent(original, newObject);
        }

        [Test]
        public void IDsInputMatchesOutput() {
            foreach (KeyValuePair<Type, string> pair in IdTable.Types) {
                Assert.AreEqual(IdTable.Types.Get(pair.Key), pair.Value);
                Assert.AreEqual(IdTable.Types.Get(pair.Value), pair.Key);
            }
            foreach (KeyValuePair<StatType, string> pair in IdTable.Stats) {
                Assert.AreEqual(IdTable.Stats.Get(pair.Key), pair.Value);
                Assert.AreEqual(IdTable.Stats.Get(pair.Value), pair.Key);
            }
            foreach (KeyValuePair<EquipType, string> pair in IdTable.Equips) {
                Assert.AreEqual(IdTable.Equips.Get(pair.Key), pair.Value);
                Assert.AreEqual(IdTable.Equips.Get(pair.Value), pair.Key);
            }
        }

        [Test]
        public void AgilitySerializationWorks() {
            IdSerializableHelper<Stat, StatSave>(new Agility(5, 25));
        }

        [Test]
        public void ItemIsSerializable() {
            IdSerializableHelper<Item, ItemSave>(new Apple());
        }

        [Test]
        public void StatTypeIsSerializable() {
            foreach (StatType type in StatType.AllTypes) {
                TypeSafeEnumSerializableHelper<StatType, StatTypeSave>(type);
            }
        }

        [Test]
        public void EquipTypeIsSerializable() {
            foreach (EquipType type in EquipType.AllTypes) {
                TypeSafeEnumSerializableHelper<EquipType, EquipTypeSave>(type);
            }
        }

        [Test]
        public void StatIsSerializable() {
            IdSerializableHelper<Stat, StatSave>(new Strength(5, 10));
        }

        [Test]
        public void CharacterStatsAreSerializable() {
            Stats stats = new Stats(1, 2, 3, 4, 5);
            ContainerSerializationHelper<Stats, CharacterStatsSave, KeyValuePair<StatType, Stat>>(stats);
        }

        [Test]
        public void SpellBookIsSerializable() {
            IdSerializableHelper<SpellBook, SpellBookSave>(new Attack());
        }

        [Test]
        public void CharacterSpellBooksAreSerializable() {
            SpellBooks sbs = new SpellBooks();
            sbs.AddSpellBook(new InflictPoison());
            sbs.AddSpellBook(new Attack());
            ContainerSerializationHelper<SpellBooks, CharacterSpellBooksSave, SpellBook>(sbs);
        }

        [Test]
        public void BrainEqualityWorks() {
            Assert.IsTrue(new Player().Equals(new Player()));
            Assert.AreEqual(new Player(), new Player());
            Assert.AreNotEqual(new Player(), new DebugAI());
        }

        [Test]
        public void CharacterBrainIsSerializable() {
            IdSerializableHelper<Brain, BrainSave>(new Player());
        }

        [Test]
        public void InventoryIsSerializable() {
            Inventory inv = new Inventory();
            inv.Add(new Apple(), 5);
            inv.Add(new PoisonArmor(), 2);
            ContainerSerializationHelper<Inventory, InventorySave, Item>(inv);
        }

        [Test]
        public void EmptyInventoryIsSerializable() {
            Inventory inv = new Inventory();
            ContainerSerializationHelper<Inventory, InventorySave, Item>(inv);
        }

        [Test]
        public void LookIsSerializable() {
            BasicSerializableHelper<Look, LookSave>(Scripts.Game.Defined.Characters.Debug.NotKitsune());
        }

        [Test]
        public void EquippedItemIsSerializable() {
            IdSerializableHelper<EquippableItem, EquipItemSave>(new PoisonArmor());
        }

        [Test]
        public void EquipmentIsSerializable() {
            Equipment equips = new Equipment();
            Inventory inv = new Inventory();
            inv.Add(new PoisonArmor());
            equips.AddEquip(inv, new BuffParams(new Stats(1, 2, 3, 4, 5), 2), new PoisonArmor());

            EquipmentSave initialSaveObject = equips.GetSaveObject();
            initialSaveObject.Buffs.ForEach(b => b.Buff.SetupAsCasterNotInParty());
            string json = ToJson(initialSaveObject);
            EquipmentSave newSaveObject = FromJson<EquipmentSave>(json);
            Equipment newObject = Util.TypeToObject<Equipment>(equips.GetType());
            newObject.InitFromSaveObject(newSaveObject);
            Assert.AreEqual(equips, newObject);
            CollectionAssert.AreEquivalent(equips, newObject);
        }

        [Test]
        public void SavingSingleCharacterPartyThrowsNoErrors() {
            SavingCharactersInPartyThrowsNoErrors(1);
        }

        [Test]
        public void SavingMultipleCharacterPartyThrowsNoErrors() {
            for (int i = 0; i < 10; i++) {
                SavingCharactersInPartyThrowsNoErrors(i);
            }
        }

        private void SavingCharactersInPartyThrowsNoErrors(int characterCount) {
            Party party = new Party();
            for (int i = 0; i < characterCount; i++) {
                party.AddMember(CharacterList.NotKitsune());
            }
            ToJson(party.GetSaveObject());
        }

        [Test]
        public void LoadingSingleCharacterPartyThrowsNoErrors() {
            LoadingCharactersInPartyThrowsNoErrors(1);
        }

        [Test]
        public void LoadingMultipleCharacterPartyThrowsNoErrors() {
            for (int i = 0; i < 10; i++) {
                LoadingCharactersInPartyThrowsNoErrors(i);
            }
        }

        private void LoadingCharactersInPartyThrowsNoErrors(int characterCount) {
            Party party = new Party();
            for (int i = 0; i < characterCount; i++) {
                party.AddMember(CharacterList.NotKitsune());
            }
            string json = ToJson(party.GetSaveObject());
            PartySave partySave = JsonUtility.FromJson<PartySave>(json);
            Party party2 = new Party();
            party2.InitFromSaveObject(partySave);
        }

        [Test]
        public void SettingCasterForBuffSetsEverythingCorrectly() {
            Character caster = CharacterList.NotKitsune();
            Buff poison = new Poison();
            poison.Caster = new BuffParams(caster.Stats, caster.Id);
            Assert.AreEqual(caster.Id, poison.CasterId);
            Assert.AreSame(caster.Stats, poison.BuffCaster);
        }

        [Test, Timeout(2000)]
        public void SaveLoadMaintainsReferencesForPartyMemberCastedBuffs() {
            Party party = new Party();
            Character dummy = CharacterList.NotKitsune();
            Character buffCaster = CharacterList.NotKitsune();
            Character buffRecipient = CharacterList.NotKitsune();

            Poison poison = new Poison();
            Util.Log("BuffcasterID: " + buffCaster.Id);
            poison.Caster = new BuffParams(buffCaster.Stats, buffCaster.Id);

            buffRecipient.Buffs.AddBuff(poison);

            party.AddMember(dummy);
            party.AddMember(buffCaster);
            party.AddMember(buffRecipient);

            PartySave retrieved = FromJson<PartySave>(ToJson(party.GetSaveObject()));

            Party party2 = new Party();
            party2.InitFromSaveObject(retrieved);

            List<Character> b = party.ToList();

            Character caster = b[1];
            Character target = b[2];

            Assert.IsTrue(party.Equals(party2));
            Assert.AreEqual(party, party2);
            Assert.AreSame(party.Collection.ToList()[1].Stats, party.Collection.ToList()[2].Buffs.ToList()[0].BuffCaster);
        }

        [Test, Timeout(2000)]
        public void SaveLoadDoesNotMaintainsReferencesForNonPartyMemberCastedBuffs() {
            Party party = new Party();
            Character dummy = CharacterList.NotKitsune();
            Character buffCaster = CharacterList.NotKitsune();
            Character buffRecipient = CharacterList.NotKitsune();

            Poison poison = new Poison();
            Util.Log("BuffcasterID: " + buffCaster.Id);
            poison.Caster = new BuffParams(buffCaster.Stats, buffCaster.Id);

            buffRecipient.Buffs.AddBuff(poison);

            party.AddMember(dummy);
            party.AddMember(CharacterList.NotKitsune());
            party.AddMember(buffRecipient);

            PartySave retrieved = FromJson<PartySave>(ToJson(party.GetSaveObject()));

            Party party2 = new Party();
            party2.InitFromSaveObject(retrieved);

            List<Character> a = party.ToList();
            List<Character> b = party.ToList();

            Character caster = b[1];
            Character target = b[2];

            Assert.IsTrue(party.Equals(party2));
            Assert.AreEqual(party, party2);
            Assert.AreNotSame(party.Collection.ToList()[1].Stats, party.Collection.ToList()[2].Buffs.ToList()[0].BuffCaster);
        }
    }
}
