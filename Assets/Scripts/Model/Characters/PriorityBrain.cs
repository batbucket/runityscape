using Scripts.Model.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scripts.Model.Spells;
using System.Linq;
using Scripts.Game.Defined.Serialized.Spells;

namespace Scripts.Model.Characters {
    public abstract class PriorityBrain : Brain {
        private static readonly SpellBook DEFAULT_ACTION = new Wait();

        private IList<Func<bool>> priorityActionList;

        public PriorityBrain() : base() {
            this.priorityActionList = SetupPriorityActions();
        }

        public sealed override void DetermineAction() {
            bool hasChosenAction = false;
            for (int i = 0; i < priorityActionList.Count && !hasChosenAction; i++) {
                Func<bool> possibleAction = priorityActionList[i];
                hasChosenAction = possibleAction();
            }
            // Unable to find anything, perform a wait so the battle doesn't get stuck
            if (!hasChosenAction) {
                handlePlay(owner.Character.Spells.CreateSpell(DEFAULT_ACTION, owner, owner));
            }
        }

        protected abstract IList<Func<bool>> SetupPriorityActions();

        protected Func<bool> CastOnRandom(SpellBook sb) {
            return CastOnTargetMeetingCondition(sb, c => true);
        }

        protected Func<bool> CastOnTargetMeetingCondition(SpellBook sb, Func<Character, bool> requirement) {
            return () => {
                Character specificTarget =
                    sb
                    .TargetType
                    .GetTargets(owner.Character, battle)
                    .Where(
                        c => sb.IsCastable(owner, new SpellParams(c)) && requirement(c))
                    .PickRandom();

                if (specificTarget != null) {
                    handlePlay(owner.Spells.CreateSpell(sb, owner, new SpellParams(specificTarget)));
                    return true;
                }
                return false;
            };
        }
    }
}
