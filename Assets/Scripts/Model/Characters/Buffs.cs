using Scripts.Model.Buffs;
using Scripts.Model.Spells;
using Scripts.Presenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using System.Collections;
using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Model.SaveLoad;

namespace Scripts.Model.Characters {
    public enum RemovalType {
        TIMED_OUT,
        DISPEL,
    }

    public class Buffs : IEnumerable<Buff>, ISaveable<CharacterBuffsSave> {

        public Stats Stats;
        public Action<SplatDetails> AddSplat;

        private HashSet<Buff> set;

        public Buffs() {
            set = new HashSet<Buff>();
            this.AddSplat = (a => { });
        }

        public void AddBuff(Buff buff) {
            buff.OnApply(Stats);
            set.Add(buff);
            AddSplat(new SplatDetails(Color.white, string.Format("+{0}", buff.Name), buff.Sprite));
        }

        public void RemoveBuff(RemovalType type, Buff buff) {
            switch (type) {
                case RemovalType.TIMED_OUT:
                    buff.OnTimeOut(Stats);
                    break;
                case RemovalType.DISPEL:
                    buff.OnDispell(Stats);
                    break;
                default:
                    Util.Assert(false, "Unknown removal type: " + type);
                    break;
            }
            set.Remove(buff);
            AddSplat(new SplatDetails(Color.grey, string.Format("-{0}", buff.Name), buff.Sprite));
        }

        IEnumerator<Buff> IEnumerable<Buff>.GetEnumerator() {
            return set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return set.GetEnumerator();
        }

        public CharacterBuffsSave GetSaveObject() {
            return new CharacterBuffsSave(set.Select(b => b.GetSaveObject()).ToList());
        }

        public void InitFromSaveObject(CharacterBuffsSave saveObject) {
            foreach (BuffSave bs in saveObject.BuffSaves) {
                Buff buff = bs.ObjectFromID();
                buff.InitFromSaveObject(bs);
                set.Add(buff);
            }
        }
    }
}
