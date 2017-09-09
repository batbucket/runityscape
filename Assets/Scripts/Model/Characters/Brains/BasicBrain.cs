using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Model.Spells;
using System;
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
            handlePlay(GetPlay());
        }

        /// <summary>
        /// Return the play you want the computer to make.
        /// </summary>
        /// <returns>A play you want the computer to make.</returns>
        protected abstract IPlayable GetPlay();

        /// <summary>
        /// Cast on a random target that meets basic conditions.
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        protected IPlayable CastOnRandom(SpellBook sb) {
            return CastOnTargetMeetingCondition(sb, c => true);
        }

        /// <summary>
        /// Cast on a random target that meets a particular requirement
        /// </summary>
        /// <param name="spellToCast">Spell to be used</param>
        /// <param name="isCharacterMeetRequirement">True if the character meets the requirements to be targeted</param>
        /// <returns></returns>
        protected IPlayable CastOnTargetMeetingCondition(SpellBook spellToCast, Func<Character, bool> isCharacterMeetRequirement) {
            Character specificTarget =
                spellToCast
                .TargetType
                .GetTargets(brainOwner.Character, currentBattle)
                .Where(
                    c => spellToCast.IsCastable(brainOwner, new SpellParams(c, currentBattle)) && isCharacterMeetRequirement(c))
                .ChooseRandom();

            // If spell is not castable, default to waiting.
            if (specificTarget != null) {
                SpellBook castableSpell = null;
                if (spellToCast.IsCastable(brainOwner, new SpellParams(specificTarget, currentBattle))) {
                    castableSpell = spellToCast;
                } else {
                    castableSpell = DEFAULT_SPELL;
                }
                return brainOwner.Spells.CreateSpell(spellToCast, brainOwner, new SpellParams(specificTarget, currentBattle));
            }
            return null;
        }
    }
}