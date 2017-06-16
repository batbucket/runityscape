using Scripts.Model.Pages;
using System;
using System.Collections.Generic;

namespace Scripts.Model.Characters {
    public abstract class Brain {
        public Character Owner;
        public SpellBooks Spells;

        protected ICollection<Character> allies;
        protected ICollection<Character> foes;

        public Brain() { }

        public void PageSetup(Battle b) {
            allies = b.GetAllies(Owner);
            foes = b.GetFoes(Owner);
            PageSetupHelper(b);
        }

        protected virtual void PageSetupHelper(Battle battle) {

        }

        public abstract void DetermineAction(Action<IPlayable> addPlay);
    }
}