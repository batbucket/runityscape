using Scripts.Game.Defined.Unserialized.Spells;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Model.Spells;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Model.Characters {
    public abstract class Brain : ISaveable<BrainSave> {
        public SpellBooks Spells;

        // Refreshed every turn
        protected ICollection<SpellParams> allies;
        protected ICollection<SpellParams> foes;

        protected SpellParams owner;
        protected Battle currentBattle;
        protected IEnumerable<ISpellable> temporarySpells;
        protected Action<IPlayable> handlePlay;

        public Brain() { }

        public void StartOfRoundSetup(Battle currentBattle, SpellParams owner) {
            this.owner = owner;
            this.currentBattle = currentBattle;
            allies = currentBattle.GetAllies(owner.Character).Select(c => new SpellParams(c, currentBattle)).ToArray();
            foes = currentBattle.GetFoes(owner.Character).Select(c => new SpellParams(c, currentBattle)).ToArray();
        }

        public void PreActionSetup(IEnumerable<ISpellable> temporarySpells, Action<IPlayable> playHandler) {
            this.temporarySpells = temporarySpells;
            this.handlePlay = playHandler;
        }

        public abstract void DetermineAction();

        public virtual string ReactToSpell(Spell spell) {
            return string.Empty;
        }

        public virtual string StartOfRoundDialogue() {
            return string.Empty;
        }

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