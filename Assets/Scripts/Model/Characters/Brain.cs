using Scripts.Model.Pages;
using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Model.Spells;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Model.Characters {
    public abstract class Brain : ISaveable<BrainSave> {
        public SpellBooks Spells;

        // Refreshed every turn
        protected ICollection<SpellParams> allies;
        protected ICollection<SpellParams> foes;

        protected SpellParams owner;
        protected Battle battle;
        protected Action<IPlayable> handlePlay;

        public Brain() { }

        public void PageSetup(Battle b, Action<IPlayable> playHandler, SpellParams owner) {
            allies = b.GetAllies(owner.Character).Select(c => new SpellParams(c)).ToArray();
            foes = b.GetFoes(owner.Character).Select(c => new SpellParams(c)).ToArray();
            this.battle = b;
            this.owner = owner;
            this.handlePlay = playHandler;
        }

        public abstract void DetermineAction();

        public override bool Equals(object obj) {
            var item = obj as Brain;

            if (item == null) {
                return false;
            }

            return this.GetType().Equals(item.GetType());
        }

        public override int GetHashCode() {
            return 0;
        }

        public BrainSave GetSaveObject() {
            return new BrainSave(GetType());
        }

        public void InitFromSaveObject(BrainSave saveObject) {
            // Nothing
        }
    }
}