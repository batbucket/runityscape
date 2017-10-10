using Scripts.Model.Acts;
using Scripts.Model.Buffs;
using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.Model.TextBoxes;
using Scripts.Model.Tooltips;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Spells {

    public class SingleSpell : Spell {
        private const string BUFF_REACT_TEXT = "<color=yellow>{0}</color>'s <color=cyan>{1}</color> activates <color=yellow>{2}</color>'s <color=cyan>{3}</color>!";

        /// <summary>
        /// The result
        /// </summary>
        private readonly Result result;

        /// <summary>
        /// The target
        /// </summary>
        private readonly Character target;

        public SingleSpell(
            SpellBook book,
            Result result,
            Character caster,
            Character target)
            : base(book, caster) {
            this.result = result;
            this.target = target;
        }

        public Result Result {
            get {
                return result;
            }
        }

        public Character Target {
            get {
                return target;
            }
        }

        public override ResultType ResultType {
            get {
                return IsCastable ? result.Type : ResultType.FAILED;
            }
        }

        public override bool IsCastable {
            get {
                return book.IsCastable(caster, new Character[] { target });
            }
        }

        protected override string TargetName {
            get {
                return target.Look.DisplayName;
            }
        }

        /// <summary>
        /// Casts this instance.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator Cast(Page current) {
            TextBox spellMessage = this.CastText;
            if (IsCastable) {
                MakeEveryonesBuffsReactToSpell(current);
                IList<IEnumerator> sfx = result.SFX;
                for (int i = 0; i < sfx.Count; i++) {
                    yield return sfx[i];
                }
                foreach (SpellEffect mySE in result.Effects) {
                    SpellEffect se = mySE;
                    se.CauseEffect();
                }
            } else {
                // Unplayable due to cast requirements becoming failed AFTER target selection due to a spellcast before this one
                // e.g the target died
                result.Type = ResultType.FAILED;
            }
            current.AddText(spellMessage);
            if (IsCastable) {
                Battle.CharacterDialogue(current, this.target, this.target.Brain.ReactToSpell(this));
            }
        }

        private void MakeEveryonesBuffsReactToSpell(Page current) {
            foreach (Character combatant in current.GetAll()) {
                foreach (Buff b in combatant.Buffs) {
                    b.ReactToSpell(current, this, combatant);
                }
            }
        }
    }
}