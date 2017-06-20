using Scripts.Model.Pages;
using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;
using System;
using System.Collections.Generic;

namespace Scripts.Model.Characters {
    public abstract class Brain : ISaveable<BrainSave> {
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

        public BrainSave GetSaveObject() {
            return new BrainSave(GetType());
        }

        public void InitFromSaveObject(BrainSave saveObject) {
            // Nothing
        }
    }
}