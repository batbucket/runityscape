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
    public class PlayGroup : IPlayable {
        public static Wait Wait = new Wait();

        private Page page;
        private Act[] preActs;
        private Act[] postActs;
        private Spell spell;

        public PlayGroup(Page page, Spell spell) {
            this.page = page;
            this.spell = spell;
            this.preActs = new Act[0];
            this.postActs = new Act[0];
        }

        public PlayGroup AddPreActs(params Act[] preActs) {
            this.preActs = preActs;
            return this;
        }

        public PlayGroup AddPostActs(params Act[] postActs) {
            this.postActs = postActs;
            return this;
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
            yield return ActUtil.SetupSceneRoutine(preActs);
            yield return spell.Play();
            yield return ActUtil.SetupSceneRoutine(postActs);
        }

        public IEnumerator Skip() {
            return spell.Play();
        }
    }
}