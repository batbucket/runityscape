using Scripts.Model.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scripts.Model.Spells;
using System.Linq;
using Scripts.Game.Defined.Serialized.Spells;

namespace Scripts.Model.Characters {
    public abstract class PriorityBrain : BasicBrain {
        private static readonly SpellBook DEFAULT_ACTION = new Wait();

        private IList<Func<IPlayable>> priorityActionList;

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
            return owner.Character.Spells.CreateSpell(DEFAULT_ACTION, owner, owner);
        }

        protected new Func<IPlayable> CastOnRandom(SpellBook sb) {
            return CastOnTargetMeetingCondition(sb, c => true);
        }

        // Doesn't cast wait if sb is uncastable (since we then go down the prioritylist)
        protected new Func<IPlayable> CastOnTargetMeetingCondition(SpellBook sb, Func<Character, bool> requirement) {
            return () => {
                Character specificTarget =
                    sb
                    .TargetType
                    .GetTargets(owner.Character, currentBattle)
                    .Where(
                        c => sb.IsCastable(owner, new SpellParams(c, currentBattle)) && requirement(c))
                    .ChooseRandom();

                if (specificTarget != null) {
                    return owner.Spells.CreateSpell(sb, owner, new SpellParams(specificTarget, currentBattle));
                }
                return null;
            };
        }

        protected abstract IList<Func<IPlayable>> SetupPriorityPlays();

    }
}
