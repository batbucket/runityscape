using Scripts.Game.Serialized;
using System;

namespace Scripts.Game.Pages {
    public class Talk {
        public readonly string Name;
        public readonly string Big;
        public Func<Flags, bool> Condition;
        public Action<Flags> SetFlags;

        public Talk(string name, string big) {
            this.Name = name;
            this.Big = big;
            this.Condition = f => true;
            this.SetFlags = f => { };
        }

        public Talk AddCondition(Func<Flags, bool> condition) {
            this.Condition = condition;
            return this;
        }

        public Talk AddSetFlags(Action<Flags> setFlags) {
            this.SetFlags = setFlags;
            return this;
        }
    }
}