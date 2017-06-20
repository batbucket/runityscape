using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Game.Defined.Spells;
using Scripts.Model.Characters;
using Scripts.Model.Spells;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Scripts.Model.Acts {
    public class PlayGroup : IPlayable {
        public static Wait Wait = new Wait();

        private IList<IPlayable> plays;
        private Spell spell;

        public PlayGroup(Character caster) {
            plays = new List<IPlayable>();
            spell = Wait.ForceSpell(caster, caster);
        }

        public bool IsPlayable {
            get {
                return true;
            }
        }

        public Spell MySpell {
            get {
                return spell;
            }
        }

        public string Text {
            get {
                return spell.SpellText;
            }
        }

        public int CompareTo(IPlayable other) {
            return this.spell.CompareTo(other.MySpell);
        }

        public IEnumerator Play() {
            for (int i = 0; i < plays.Count; i++) {
                yield return plays[i].Play();
                // TODO between each one allow a step button to proceed???
            }
        }

        public IEnumerator Skip() {
            return spell.Play();
        }
    }
}