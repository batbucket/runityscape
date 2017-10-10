using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Model.Spells;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Model.Characters {

    /// <summary>
    /// A simplified AI that only requires an IPlayable to be returned.
    /// If one is not returned, it defaults to Wait being cast.
    /// </summary>
    public abstract class BasicBrain : Brain {
        private static readonly SpellBook DEFAULT_SPELL = new Wait();

        /// <summary>
        /// Action is determined by whatever the play is.
        /// </summary>
        public sealed override void DetermineAction() {
            spellHandler(GetSpell());
        }

        /// <summary>
        /// Return the play you want the computer to make.
        /// </summary>
        /// <returns>A play you want the computer to make.</returns>
        protected abstract Spell GetSpell();

        /// <summary>
        /// Cast on a random target that meets basic conditions.
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        protected Spell CastOnRandom(SpellBook sb) {
            Spell spell = null;

            if (sb.TargetType.TargetCount == TargetCount.SINGLE_TARGET) {
                spell = CastOnLeastTarget(sb, c => 0);
            } else if (sb.TargetType.TargetCount == TargetCount.MULTIPLE_TARGETS) {
                spell = brainOwner.Spells.CreateSpell(currentBattle, sb, brainOwner);
            } else {
                Util.Assert(false, "Invalid target count: " + sb.TargetType.TargetCount);
            }

            return spell ?? brainOwner.Spells.CreateSpell(currentBattle, DEFAULT_SPELL, brainOwner, brainOwner);
        }

        protected Spell CastOnLeastTarget(SpellBook spellToCast, Func<Character, int> sorter) {
            return CastOnLeastTarget(spellToCast, sorter, DEFAULT_SPELL);
        }

        protected Spell CastOnTargetMeetingCondition(SpellBook spellToCast, Func<Character, bool> isCharacterMeetCondition) {
            Util.Assert(spellToCast.TargetType.TargetCount == TargetCount.SINGLE_TARGET, "Spell is not single target.");
            IList<Character> possibleTargets =
                spellToCast
                .TargetType
                .GetTargets(brainOwner, currentBattle)
                .Where(c => spellToCast.IsCastable(brainOwner, new Character[] { c }) && isCharacterMeetCondition(c)).ToArray();
            possibleTargets.Shuffle();
            return GetSpellOrDefault(possibleTargets.FirstOrDefault(), spellToCast, DEFAULT_SPELL);
        }

        /// <summary>
        /// Cast on a random target that is the least according to a sorter function.
        /// Guarenteed to pick a target, so if this is used in PriorityBrain someone (even if they're full health)
        /// will get healed.
        /// </summary>
        /// <param name="spellToCast">Spell to be used</param>
        /// <param name="isCharacterMeetRequirement">Sorter function to sort the list of targets</param>
        /// <returns>Spell cast on the least target</returns>
        protected Spell CastOnLeastTarget(SpellBook spellToCast, Func<Character, int> sorter, SpellBook defaultSpell) {
            return CastOnTarget(spellToCast, (characters) => characters.OrderBy(c => sorter(c)).FirstOrDefault(), defaultSpell);
        }

        protected Spell CastOnTarget(SpellBook spellToCast, Func<IEnumerable<Character>, Character> singleCharacterChooser, SpellBook defaultSpell) {
            Util.Assert(spellToCast.TargetType.TargetCount == TargetCount.SINGLE_TARGET, "Spell is not single target.");

            IList<Character> possibleTargets =
                spellToCast
                .TargetType
                .GetTargets(brainOwner, currentBattle)
                .Where(c => spellToCast.IsCastable(brainOwner, new Character[] { c })).ToArray();
            possibleTargets.Shuffle();

            return GetSpellOrDefault(singleCharacterChooser(possibleTargets), spellToCast, defaultSpell);
        }

        private Spell GetSpellOrDefault(Character specificTarget, SpellBook spellToCast, SpellBook defaultSpell) {
            // If spell is not castable, default to waiting.
            if (specificTarget != null) {
                SpellBook castableSpell = null;
                if (spellToCast.IsCastable(brainOwner, new Character[] { specificTarget })) {
                    castableSpell = spellToCast;
                } else {
                    castableSpell = defaultSpell;
                }
                if (castableSpell != null) {
                    return brainOwner.Spells.CreateSpell(currentBattle, spellToCast, brainOwner, specificTarget);
                }
            }
            return brainOwner.Spells.CreateSpell(currentBattle, DEFAULT_SPELL, brainOwner, brainOwner);
        }
    }
}