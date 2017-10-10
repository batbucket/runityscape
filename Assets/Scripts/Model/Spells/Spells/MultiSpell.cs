using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Model.TextBoxes;
using Scripts.Model.Characters;
using Scripts.Model.Pages;

namespace Scripts.Model.Spells {

    public class MultiSpell : Spell {
        private readonly IEnumerable<SingleSpell> spells;

        public MultiSpell(SpellBook book, Character caster, IEnumerable<SingleSpell> spells) : base(book, caster) {
            this.spells = spells;
        }

        public override ResultType ResultType {
            get {
                return IsCastable ? ResultType.HIT : ResultType.FAILED;
            }
        }

        public override bool IsCastable {
            get {
                return spells.Any(s1 => book.IsCastableIgnoreResources(caster, spells.Select(s2 => s2.Target).ToArray()));
            }
        }

        protected override string TargetName {
            get {
                return string.Format(" on <color=yellow>{0}</color>", book.TargetType.Name.ToLower());
            }
        }

        protected override IEnumerator Cast(Page current, bool isAddCastText) {
            TextBox spellMessage = this.CastText;
            if (IsCastable) {
                foreach (SingleSpell spell in spells) {
                    yield return spell.Play(current, false);
                }
            }
            if (isAddCastText) {
                current.AddText(spellMessage);
            }
        }
    }
}