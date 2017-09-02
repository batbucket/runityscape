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

    public class Equipment
        :
            IEnumerable<Item>,
            IEnumerable<EquippableItem>,
            IEnumerable<ISpellable>,
            IEnumerable<KeyValuePair<EquipType, EquippableItem>>,
            IEnumerable<KeyValuePair<EquipType, Buff>>,
            IEnumerable<KeyValuePair<StatType, int>>,
            ISaveable<EquipmentSave> {
        public Action<Buff> AddBuff;
        public Action<Buff> RemoveBuff;
        public Action<SplatDetails> AddSplat;

        private readonly IDictionary<EquipType, EquippableItem> equipped;
        private readonly IDictionary<EquipType, Buff> itemBuffs;
        private readonly IDictionary<StatType, int> statBonuses;

        // Temporary serialization fields
        private bool IsSetupDone;
        private List<Character> partyMembers;

        public Equipment() {
            this.equipped = new Dictionary<EquipType, EquippableItem>();
            this.statBonuses = new Dictionary<StatType, int>();
            this.AddBuff = ((a) => { });
            this.RemoveBuff = ((a) => { });
            foreach (StatType s in StatType.ASSIGNABLES) {
                statBonuses.Add(s, 0);
            }
            this.itemBuffs = new Dictionary<EquipType, Buff>();
            this.AddSplat = ((p) => { });
        }

        public void AddEquip(Inventory inv, BuffParams caster, EquippableItem e) {
            inv.Remove(e);
            if (Contains(e.Type)) {
                RemoveEquip(inv, e.Type);
            }

            Buff buff = e.CreateBuff();
            if (buff != null) {
                buff.Caster = caster;
                itemBuffs.Add(e.Type, buff);
                AddBuff(buff);
            }

            equipped.Add(e.Type, e);
            foreach (KeyValuePair<StatType, int> pair in e.Stats) {
                Util.Assert(StatType.ASSIGNABLES.Contains(pair.Key), "Invalid stat type on equipment.");
                this.statBonuses[pair.Key] += pair.Value;
            }
        }

        public void RemoveEquip(Inventory inventory, EquipType type) {
            Util.Assert(equipped.ContainsKey(type), "No equipment in slot.");
            EquippableItem itemToRemove = equipped[type];

            if (itemBuffs.ContainsKey(itemToRemove.Type)) {
                Buff buffToRemove = itemBuffs[itemToRemove.Type];
                itemBuffs.Remove(itemToRemove.Type);
                RemoveBuff(buffToRemove);
            }

            inventory.Add(itemToRemove);
            equipped.Remove(itemToRemove.Type);

            foreach (KeyValuePair<StatType, int> pair in itemToRemove.Stats) {
                Util.Assert(StatType.ASSIGNABLES.Contains(pair.Key), "Invalid stat type on equipment.");
                this.statBonuses[pair.Key] -= pair.Value;
            }
        }

        public EquippableItem PeekItem(EquipType type) {
            return equipped[type];
        }

        public int GetBonus(StatType type) {
            return statBonuses.ContainsKey(type) ? statBonuses[type] : 0;
        }

        public bool Contains(EquipType type) {
            return equipped.ContainsKey(type);
        }

        public override bool Equals(object obj) {
            var item = obj as Equipment;

            if (item == null) {
                return false;
            }

            return Util.IsDictionariesEqual(this.equipped, item.equipped)
                && Util.IsDictionariesEqual(this.itemBuffs, item.itemBuffs)
                && Util.IsDictionariesEqual(this.statBonuses, item.statBonuses);
        }

        public bool HasEquip(EquipType type) {
            return equipped.ContainsKey(type);
        }

        public override int GetHashCode() {
            return 0;
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
            foreach (ISaveable<EquipItemSave> eq in equipped.Values) {
                equips.Add(eq.GetSaveObject());
            }

            List<EquipmentSave.EquipBonus> bonuses = new List<EquipmentSave.EquipBonus>();
            foreach (KeyValuePair<StatType, int> pair in statBonuses) {
                bonuses.Add(new EquipmentSave.EquipBonus() { Stat = new StatTypeSave(pair.Key), Bonus = pair.Value });
            }

            List<EquipmentSave.EquipBuff> buffs = new List<EquipmentSave.EquipBuff>();
            foreach (KeyValuePair<EquipType, Buff> pair in itemBuffs) {
                buffs.Add(new EquipmentSave.EquipBuff() {
                    EquipType = new EquipTypeSave(pair.Key),
                    Buff = new BuffSave(
                        pair.Value.TurnsRemaining,
                        pair.Value.BuffCaster.GetSaveObject(),
                        pair.Value.CasterId, pair.Value.GetType())
                });
            }
            return new EquipmentSave(equips, buffs, bonuses);
        }

        public void InitFromSaveObject(EquipmentSave saveObject) {
            foreach (EquipItemSave save in saveObject.Equipped) {
                EquippableItem eq = save.CreateObjectFromID();
                eq.InitFromSaveObject(save);
                equipped.Add(save.EquipTypeSave.Restore(), eq);
            }

            statBonuses.Clear();
            foreach (EquipmentSave.EquipBonus bonus in saveObject.Bonuses) {
                StatType type = bonus.Stat.Restore();
                int count = bonus.Bonus;
                this.statBonuses.Add(type, count);
            }

            foreach (EquipmentSave.EquipBuff eb in saveObject.Buffs) {
                BuffSave bs = eb.Buff;
                EquipType et = eb.EquipType.Restore();
                Buff buff = CharacterBuffsSave.SetupBuffCasterFromSave(bs, partyMembers);
                itemBuffs.Add(et, buff);
            }

            partyMembers = null;
        }

        public void SetupTemporarySaveFields(List<Character> partyMembers) {
            Util.Assert(!IsSetupDone, "Only callable when restoring Equipment in Party.");
            this.partyMembers = partyMembers;
            IsSetupDone = true;
        }

        IEnumerator<KeyValuePair<EquipType, EquippableItem>> IEnumerable<KeyValuePair<EquipType, EquippableItem>>.GetEnumerator() {
            return equipped.GetEnumerator();
        }

        IEnumerator<KeyValuePair<EquipType, Buff>> IEnumerable<KeyValuePair<EquipType, Buff>>.GetEnumerator() {
            return itemBuffs.GetEnumerator();
        }

        IEnumerator<KeyValuePair<StatType, int>> IEnumerable<KeyValuePair<StatType, int>>.GetEnumerator() {
            return statBonuses.GetEnumerator();
        }

        IEnumerator<Item> IEnumerable<Item>.GetEnumerator() {
            return equipped.Cast<Item>().GetEnumerator();
        }
    }
}
