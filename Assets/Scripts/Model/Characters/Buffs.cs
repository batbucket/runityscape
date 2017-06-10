using Scripts.Model.Buffs;
using Scripts.Model.Spells;
using Scripts.Presenter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Characters {
    public enum RemovalType {
        TIMED_OUT,
        DISPELL
    }

    public class Buffs {
        public ICollection<Buff> Collection;

        public Action<SplatDetails> AddSplat;

        public Buffs() {
            Collection = new HashSet<Buff>();
            this.AddSplat = (a => { });
        }

        public void AddBuff(Buff buff) {
            buff.OnApply();
            Collection.Add(buff);
            AddSplat(new SplatDetails(Color.white, string.Format("+{0}", buff.Name), buff.Sprite));
        }

        public void RemoveBuff(RemovalType type, Buff buff) {
            switch (type) {
                case RemovalType.TIMED_OUT:
                    buff.OnTimeOut();
                    break;
                case RemovalType.DISPELL:
                    buff.OnDispell();
                    break;
                default:
                    Util.Assert(false, "Unknown removal type: " + type);
                    break;
            }
            Collection.Remove(buff);
            AddSplat(new SplatDetails(Color.grey, string.Format("-{0}", buff.Name), buff.Sprite));
        }
    }
}
