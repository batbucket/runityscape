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

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityBrain"/> class.
        /// </summary>
        public PriorityBrain() : base() { }

        protected sealed override Spell GetSpell() {
            Spell chosenPlay = null;
            foreach (Spell spell in GetPriorityPlays()) {
                chosenPlay = spell;
                if (!(chosenPlay.SpellBook).Equals(DEFAULT_ACTION)) {
                    return chosenPlay;
                }
            }
            // Unable to find anything, perform a wait so the battle doesn't get stuck
            return brainOwner.Spells.CreateSpell(currentBattle, DEFAULT_ACTION, brainOwner, brainOwner);
        }

        /// <summary>
        /// Setups the priority plays.
        /// </summary>
        /// <returns></returns>
        protected abstract IList<Spell> GetPriorityPlays();
    }
}