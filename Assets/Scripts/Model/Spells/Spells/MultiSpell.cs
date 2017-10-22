﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Model.TextBoxes;
using Scripts.Model.Characters;
using Scripts.Model.Pages;

namespace Scripts.Model.Spells {

    /// <summary>
    /// Represents a spell that hits multiple targets
    /// </summary>
    public class MultiSpell : Spell {
        private readonly Func<Page, Character, Character, Spell> spellBuilder;
        private readonly Page current;

        public MultiSpell(SpellBook book, Character caster, Page current, Func<Page, Character, Character, Spell> spellBuilder) : base(book, caster) {
            this.spellBuilder = spellBuilder;
            this.current = current;
        }

        public override ResultType ResultType {
            get {
                return IsCastable ? ResultType.HIT : ResultType.FAILED;
            }
        }

        public override bool IsCastable {
            get {
                return book.TargetType.GetTargets(caster, current).Any(target => book.IsCastableIgnoreResources(caster, new Character[] { target }));
            }
        }

        protected override string TargetName {
            get {
                return string.Format(" on <color=yellow>{0}</color>", book.TargetType.Name.ToLower());
            }
        }

        private IEnumerable<Character> Targets {
            get {
                return book.TargetType.GetTargets(caster, current);
            }
        }

        protected override IEnumerator Cast(Page current, bool isAddCastText) {
            TextBox spellMessage = this.CastText;
            if (IsCastable) {
                foreach (Character target in Targets) {
                    yield return spellBuilder(current, caster, target).Play(current, false);
                }
            }
            if (isAddCastText) {
                current.AddText(spellMessage);
            }
        }
    }
}