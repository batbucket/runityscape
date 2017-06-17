using Scripts.Model.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scripts.Presenter;

namespace Scripts.Model.Spells {

    public class Spell : IPlayable, IComparable<Spell> {
        public readonly SpellBook Book;
        public readonly Result Result;
        public readonly Character Caster;
        public readonly Character Target;

        public Spell(SpellBook book, Result result, Character caster, Character target) {
            this.Book = book;
            this.Result = result;
            this.Caster = caster;
            this.Target = target;
        }

        public string SpellDeclareText {
            get {
                return string.Format("<color=yellow>{0}</color> will use <color=cyan>{1}</color>{2}.",
                  Caster.Look.DisplayName,
                  Book.Name,
                  Caster.Equals(Target) ? string.Empty : string.Format(" on <color=yellow>{0}</color>", Target.Look.DisplayName)
              );
            }
        }

        public string SpellText {
            get {
                return string.Format("<color=yellow>{0}</color> uses <color=cyan>{1}</color>{2}.{3}",
                  Caster.Look.DisplayName,
                  Book.Name,
                  Caster.Equals(Target) ? string.Empty : string.Format(" on <color=yellow>{0}</color>", Target.Look.DisplayName),
                  Result.Type.Text
              );
            }
        }

        public bool IsPlayable {
            get {
                return Book.IsCastable(new SpellParams(Caster), new SpellParams(Target));
            }
        }

        public Spell MySpell {
            get {
                return this;
            }
        }

        public string Text {
            get {
                return SpellText;
            }
        }

        public IEnumerator Skip() {
            foreach (SpellEffect se in Result.Effects) {
                se.CauseEffect();
            }
            yield break;
        }

        public IEnumerator Play() {
            return Cast();
        }

        public int CompareTo(Spell other) {
            int diff = other.Book.Priority - this.Book.Priority;
            if (diff == 0) {
                return this.Caster.CompareTo(other.Caster);
            } else {
                return diff;
            }
        }

        private IEnumerator Cast() {
            if (IsPlayable) {
                IList<IEnumerator> sfx = Result.SFX;
                for (int i = 0; i < sfx.Count; i++) {
                    yield return sfx[i];
                }
                foreach (SpellEffect mySE in Result.Effects) {
                    SpellEffect se = mySE;
                    se.CauseEffect();
                }
            } else {
                // Unplayable due to cast requirements becoming failed AFTER target selection due to a spellcast before this one
                Result.Type = ResultType.FAILED;
            }
        }

        public int CompareTo(IPlayable other) {
            return this.CompareTo(other.MySpell);
        }
    }
}