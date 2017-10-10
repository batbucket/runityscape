using Scripts.Model.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scripts.Presenter;
using Scripts.Model.TextBoxes;
using Scripts.Model.Tooltips;
using Scripts.Model.Pages;

namespace Scripts.Model.Spells {

    /// <summary>
    /// Spells result from a spellbook being used on a target by a caster
    /// </summary>
    /// <seealso cref="IPlayable" />
    /// <seealso cref="System.IComparable{Scripts.Model.Spells.Spell}" />
    public abstract class Spell : IComparable<Spell> {

        /// <summary>
        /// The spell text
        /// </summary>
        public const string CAST_TEXT = "<color=yellow>{0}</color> {1}s <color=cyan>{2}</color>{3}.{4}";

        public const string DECLARE_TEXT = "<color=yellow >{0}</color> will {1} <color=cyan>{2}</color>{3}.";

        /// <summary>
        /// The book
        /// </summary>
        protected readonly SpellBook book;

        /// <summary>
        /// The caster of this spell
        /// </summary>
        protected readonly Character caster;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spell"/> class.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="caster">The caster.</param>
        public Spell(SpellBook book, Character caster) {
            this.book = book;
            this.caster = caster;
        }

        public Character Caster {
            get {
                return caster;
            }
        }

        public string CasterName {
            get {
                return caster.Look.DisplayName;
            }
        }

        /// <summary>
        /// Gets the spell text.
        /// </summary>
        /// <value>
        /// The spell text.
        /// </value>
        public TextBox CastText {
            get {
                return new TextBox(
                    string.Format(
                        CAST_TEXT,
                        caster.Look.DisplayName,
                        book.Verb.ToLower(),
                        book.Name,
                        TargetName,
                        ResultType.Text),
                    book.TextboxTooltip
                    );
            }
        }

        public TextBox DeclareText {
            get {
                return new TextBox(string.Format(DECLARE_TEXT,
                  caster.Look.DisplayName,
                  book.Verb.ToLower(),
                  book.Name,
                  TargetName),
                  book.TextboxTooltip
              );
            }
        }

        public abstract ResultType ResultType {
            get;
        }

        public SpellBook SpellBook {
            get {
                return book;
            }
        }

        public abstract bool IsCastable {
            get;
        }

        protected abstract string TargetName {
            get;
        }

        /// <summary>
        /// Compares to another spell to determine which occurs first.
        /// </summary>
        /// <param name="other">The other spell.</param>
        /// <returns></returns>
        public int CompareTo(Spell other) {
            int diff = other.book.Priority - this.book.Priority; // Descending comparison by priority. 1 will go before -1.
            if (diff == 0) {
                return this.caster.Stats.CompareTo(other.caster.Stats);
            } else {
                return diff;
            }
        }

        /// <summary>
        /// Plays this instance.
        /// </summary>
        /// <returns>
        /// Coroutine
        /// </returns>
        public IEnumerator Play(Page current, bool isAddCastText) {
            yield return Cast(current, isAddCastText);
        }

        protected abstract IEnumerator Cast(Page current, bool isAddCastText);
    }
}