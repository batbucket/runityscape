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

namespace Scripts.Model.Characters {

    public class Equipment : IEnumerable<EquippableItem>, IEnumerable<SpellBook> {
        public readonly IDictionary<EquipType, EquippableItem> Dict;
        public Inventory Inventory;
        public Buffs Buffs;
        public SpellParams Owner;
        public Action<SplatDetails> AddSplat;

        private readonly IDictionary<EquippableItem, Buff> itemBuffs;
        private readonly IDictionary<StatType, int> statBonuses;

        public Equipment() {
            this.Dict = new Dictionary<EquipType, EquippableItem>();
            this.statBonuses = new Dictionary<StatType, int>();
            foreach (StatType s in StatType.ASSIGNABLES) {
                statBonuses.Add(s, 0);
            }
            this.itemBuffs = new Dictionary<EquippableItem, Buff>();
            this.AddSplat = ((p) => { });
        }

        public void AddEquip(EquippableItem e) {
            Inventory.Remove(e);
            if (Contains(e.Type)) {
                RemoveEquip(e.Type);
            }

            Buff buff = e.CreateBuff(Owner);
            if (buff != null) {
                itemBuffs.Add(e, buff);
                Buffs.AddBuff(buff);
            }

            Dict.Add(e.Type, e);
            foreach (KeyValuePair<StatType, int> pair in e.Stats) {
                Util.Assert(StatType.ASSIGNABLES.Contains(pair.Key), "Invalid stat type on equipment.");
                this.statBonuses[pair.Key] += pair.Value;
            }

            AddSplat(new SplatDetails(Color.green, string.Format("{0}+{1}", e.Type.Name, e.Name), e.Icon));
        }

        public void RemoveEquip(EquipType type) {
            Util.Assert(Dict.ContainsKey(type), "No equipment in slot.");
            EquippableItem itemToRemove = Dict[type];

            Buff buffToRemove = itemBuffs[itemToRemove];
            itemBuffs.Remove(itemToRemove);

            Inventory.Add(itemToRemove);
            Dict.Remove(itemToRemove.Type);
            Buffs.RemoveBuff(RemovalType.TIMED_OUT, buffToRemove);

            foreach (KeyValuePair<StatType, int> pair in itemToRemove.Stats) {
                Util.Assert(StatType.ASSIGNABLES.Contains(pair.Key), "Invalid stat type on equipment.");
                this.statBonuses[pair.Key] -= pair.Value;
            }

            AddSplat(new SplatDetails(Color.red, string.Format("{0}-{1}", itemToRemove.Type.Name, itemToRemove.Name), itemToRemove.Icon));
        }

        public EquippableItem PeekItem(EquipType type) {
            return Dict[type];
        }

        public int GetBonus(StatType type) {
            return statBonuses[type];
        }

        public bool Contains(EquipType type) {
            return Dict.ContainsKey(type);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Dict.Values.GetEnumerator();
        }

        IEnumerator<EquippableItem> IEnumerable<EquippableItem>.GetEnumerator() {
            return Dict.Values.GetEnumerator();
        }

        IEnumerator<SpellBook> IEnumerable<SpellBook>.GetEnumerator() {
            return Dict.Values.Select(e => e.GetSpellBook()).ToList().GetEnumerator();
        }
    }
}
