using Scripts.Model.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Scripts.Model.Stats;
using Scripts.Model.Buffs;
using Scripts.Model.Spells;
using Scripts.Presenter;
using Scripts.Model.Interfaces;
using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;

namespace Scripts.Model.Characters {

    public class Equipment : IEnumerable<EquippableItem>, IEnumerable<ISpellable>, ISaveable<EquipmentSave, EquipmentSave> {
        public Action<Buff> AddBuff;
        public Action<Buff> RemoveBuff;
        public Action<SplatDetails> AddSplat;

        private readonly IDictionary<EquipType, EquippableItem> equipped;
        private readonly IDictionary<EquippableItem, Buff> itemBuffs;
        private readonly IDictionary<StatType, int> statBonuses;

        public Equipment() {
            this.equipped = new Dictionary<EquipType, EquippableItem>();
            this.statBonuses = new Dictionary<StatType, int>();
            this.AddBuff = ((a) => { });
            this.RemoveBuff = ((a) => { });
            foreach (StatType s in StatType.ASSIGNABLES) {
                statBonuses.Add(s, 0);
            }
            this.itemBuffs = new Dictionary<EquippableItem, Buff>();
            this.AddSplat = ((p) => { });
        }

        public void AddEquip(Inventory inv, EquippableItem e) {
            inv.Remove(e);
            if (Contains(e.Type)) {
                RemoveEquip(inv, e.Type);
            }

            Buff buff = e.CreateBuff();
            if (buff != null) {
                itemBuffs.Add(e, buff);
                AddBuff(buff);
            }

            equipped.Add(e.Type, e);
            foreach (KeyValuePair<StatType, int> pair in e.Stats) {
                Util.Assert(StatType.ASSIGNABLES.Contains(pair.Key), "Invalid stat type on equipment.");
                this.statBonuses[pair.Key] += pair.Value;
            }

            AddSplat(new SplatDetails(Color.green, "+", e.Icon));
        }

        public void RemoveEquip(Inventory inv, EquipType type) {
            Util.Assert(equipped.ContainsKey(type), "No equipment in slot.");
            EquippableItem itemToRemove = equipped[type];

            Buff buffToRemove = itemBuffs[itemToRemove];
            itemBuffs.Remove(itemToRemove);

            inv.Add(itemToRemove);
            equipped.Remove(itemToRemove.Type);
            RemoveBuff(buffToRemove);

            foreach (KeyValuePair<StatType, int> pair in itemToRemove.Stats) {
                Util.Assert(StatType.ASSIGNABLES.Contains(pair.Key), "Invalid stat type on equipment.");
                this.statBonuses[pair.Key] -= pair.Value;
            }

            AddSplat(new SplatDetails(Color.red, "-", itemToRemove.Icon));
        }

        public EquippableItem PeekItem(EquipType type) {
            return equipped[type];
        }

        public int GetBonus(StatType type) {
            return statBonuses[type];
        }

        public bool Contains(EquipType type) {
            return equipped.ContainsKey(type);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return equipped.Values.GetEnumerator();
        }

        IEnumerator<EquippableItem> IEnumerable<EquippableItem>.GetEnumerator() {
            return equipped.Values.GetEnumerator();
        }

        IEnumerator<ISpellable> IEnumerable<ISpellable>.GetEnumerator() {
            return equipped.Values.Select(e => e.GetSpellBook()).Cast<ISpellable>().ToList().GetEnumerator();
        }

        public EquipmentSave GetSaveObject() {
            List<EquipItemSave> equips = new List<EquipItemSave>();
            foreach (ISaveable<EquipItemSave, EquipItemSave> eq in equipped.Values) {
                equips.Add(eq.GetSaveObject());
            }

            List<EquipmentSave.EquipBonus> bonuses = new List<EquipmentSave.EquipBonus>();
            foreach (KeyValuePair<StatType, int> pair in statBonuses) {
                bonuses.Add(new EquipmentSave.EquipBonus() { Stat = IDs.Stats.Get(pair.Key), Bonus = pair.Value });
            }

            // TODO buffs
            return new EquipmentSave(equips, bonuses);
        }

        public void InitFromSaveObject(EquipmentSave saveObject) {
            foreach (EquipItemSave save in saveObject.Equipped) {
                EquippableItem eq = save.CreateObjectFromID();
                eq.InitFromSaveObject(save);
                equipped.Add(save.EquipTypeSave.Restore(), eq);
            }

            foreach (EquipmentSave.EquipBonus bonus in saveObject.Bonuses) {
                StatType type = IDs.Stats.Get(bonus.Stat);
                int count = bonus.Bonus;
                this.statBonuses.Add(type, count);
            }

            // TODO buffs
        }
    }
}
