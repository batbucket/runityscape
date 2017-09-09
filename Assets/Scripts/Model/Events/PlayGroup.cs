using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Game.Defined.Spells;
using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.Model.Spells;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Model.Acts {
    /// <summary>
    /// Allows for NPCs to say something before and after they use a spell.
    /// </summary>
    /// <seealso cref="IPlayable" />
    public class PlayGroup : IPlayable {
        public static Wait Wait = new Wait();

        private Page current;
        private Act[] preActs;
        private Act[] postActs;
        private Spell spell;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayGroup"/> class.
        /// </summary>
        /// <param name="current">The page the playgroup is occuring on.</param>
        /// <param name="spell">The spell.</param>
        public PlayGroup(Page current, Spell spell) {
            this.current = current;
            this.spell = spell;
            this.preActs = new Act[0];
            this.postActs = new Act[0];
        }

        /// <summary>
        /// Adds the pre acts. Done before the spell is cast.
        /// </summary>
        /// <param name="preActs">The pre acts.</param>
        /// <returns></returns>
        public PlayGroup AddPreActs(params Act[] preActs) {
            this.preActs = preActs;
            return this;
        }

        /// <summary>
        /// Adds the post acts. Done after the spell is cast.
        /// </summary>
        /// <param name="postActs">The post acts.</param>
        /// <returns></returns>
        public PlayGroup AddPostActs(params Act[] postActs) {
            this.postActs = postActs;
            return this;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is playable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is playable; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlayable {
            get {
                return true;
            }
        }

        /// <summary>
        /// Gets the spell associated with the action.
        /// </summary>
        /// <value>
        /// My spell.
        /// </value>
        public Spell MySpell {
            get {
                return spell;
            }
        }

        /// <summary>
        /// Gets the text associated with the action.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text {
            get {
                return spell.SpellText;
            }
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public int CompareTo(IPlayable other) {
            return this.spell.CompareTo(other.MySpell);
        }

        /// <summary>
        /// Plays this instance.
        /// </summary>
        /// <returns>
        /// Coroutine
        /// </returns>
        public IEnumerator Play() {
            yield return ActUtil.SetupSceneRoutine(preActs);
            yield return spell.Play();
            yield return ActUtil.SetupSceneRoutine(postActs);
        }

        /// <summary>
        /// Skips this instance.
        /// </summary>
        /// <returns>
        /// Coroutine.
        /// </returns>
        public IEnumerator Skip() {
            return spell.Play();
        }
    }
}