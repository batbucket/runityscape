using Scripts.Model.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scripts.Presenter;

namespace Scripts.Model.Spells {

    public class Spell : IPlayable, IComparable<Spell> {
        public const string SPELL_TEXT = "<color=yellow>{0}</color> {1} <color=cyan>{2}</color>{3}.{4}";
        public readonly SpellBook Book;
        public readonly Result Result;
        public readonly SpellParams Caster;
        public readonly SpellParams Target;

        public Spell(SpellBook book, Result result, SpellParams caster, SpellParams target) {
            this.Book = book;
            this.Result = result;
            this.Caster = caster;
            this.Target = target;
        }

        public string SpellDeclareText {
            get {
                return string.Format("<color=yellow>{0}</color> will {1} <color=cyan>{2}</color>{3}.",
                  Caster.Look.DisplayName,
                  Book.Verb.ToLower(),
                  Book.Name,
                  Caster.Equals(Target) ? string.Empty : string.Format(" on <color=yellow>{0}</color>", Target.Look.DisplayName)
              );
            }
        }

        public string SpellText {
            get {
                return GetCastMessage(Caster, Target, this.Book, this.Result.Type);
            }
        }

        public bool IsPlayable {
            get {
                return Book.IsCastable(Caster, Target);
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

        public static string GetCastMessage(SpellParams caster, SpellParams target, SpellBook book, ResultType result) {
            return string.Format("<color=yellow>{0}</color> {1} <color=cyan>{2}</color>{3}.{4}",
              caster.Look.DisplayName,
              book.Verb.ToLower() + 's',
              book.Name,
              caster.Equals(target) ? string.Empty : string.Format(" on <color=yellow>{0}</color>", target.Look.DisplayName),
              result.Text
          );
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
            int diff = other.Book.Priority - this.Book.Priority; // 1 - (-1) = 2
            if (diff == 0) {
                return this.Caster.Stats.CompareTo(other.Caster.Stats);
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