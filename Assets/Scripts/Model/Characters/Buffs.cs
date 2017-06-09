using Scripts.Model.Buffs;
using System.Collections;
using System.Collections.Generic;

namespace Scripts.Model.Characters {
    public enum RemovalType {
        TIMED_OUT,
        DISPELL
    }

    public class Buffs {
        public ICollection<Buff> Collection;

        public Buffs() {
            Collection = new HashSet<Buff>();
        }

        public void AddBuff(Buff buff) {
            buff.OnApply();
            Collection.Add(buff);
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
        }
    }
}
