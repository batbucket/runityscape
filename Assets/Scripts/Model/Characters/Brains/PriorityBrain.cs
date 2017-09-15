using Scripts.Model.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scripts.Model.Spells;
using System.Linq;
using Scripts.Game.Defined.Serialized.Spells;

namespace Scripts.Model.Characters {

    /// <summary>
    /// A priority brain bases its decisions on a fallthrough list, stopping
    /// at the first condition that evaluates to true.
    /// </summary>
    /// <seealso cref="Scripts.Model.Characters.BasicBrain" />
    public abstract class PriorityBrain : BasicBrain {
        private static readonly SpellBook DEFAULT_ACTION = new Wait();

        private IList<Func<IPlayable>> priorityActionList;

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityBrain"/> class.
        /// </summary>
        public PriorityBrain() : base() {
            this.priorityActionList = SetupPriorityPlays();
        }

        protected sealed override IPlayable GetPlay() {
            IPlayable chosenPlay = null;
            for (int i = 0; i < priorityActionList.Count; i++) {
                chosenPlay = priorityActionList[i]();
                if (chosenPlay != null) {
                    return chosenPlay;
                }
            }
            // Unable to find anything, perform a wait so the battle doesn't get stuck
            return brainOwner.Spells.CreateSpell(currentBattle, DEFAULT_ACTION, brainOwner, brainOwner);
        }

        protected new Func<IPlayable> CastOnRandom(SpellBook sb) {
            return CastOnTargetMeetingCondition(sb, c => true);
        }

        /// <summary>
        /// Doesn't cast wait if spellbook is uncastable (since we then go down the prioritylist)
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="requirement">The requirement.</param>
        /// <returns></returns>
        protected new Func<IPlayable> CastOnTargetMeetingCondition(SpellBook sb, Func<Character, bool> requirement) {
            return () => {
                Character specificTarget =
                    sb
                    .TargetType
                    .GetTargets(brainOwner, currentBattle)
                    .Where(
                        c => sb.IsCastable(brainOwner, c) && requirement(c))
                    .ChooseRandom();

                if (specificTarget != null) {
                    return brainOwner.Spells.CreateSpell(currentBattle, sb, brainOwner, specificTarget);
                }
                return null;
            };
        }

        /// <summary>
        /// Setups the priority plays.
        /// </summary>
        /// <returns></returns>
        protected abstract IList<Func<IPlayable>> SetupPriorityPlays();
    }
}