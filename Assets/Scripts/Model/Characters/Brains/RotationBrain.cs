using Scripts.Model.Spells;
using System;
using System.Collections.Generic;

namespace Scripts.Model.Characters {

    /// <summary>
    /// Character has a specific rotation that it always does.
    /// </summary>
    /// <seealso cref="Scripts.Model.Characters.BasicBrain" />
    public abstract class RotationBrain : BasicBrain {
        private IList<Func<Spell>> rotation;

        public RotationBrain() {
            this.rotation = GetRotation();
        }

        protected sealed override Spell GetSpell() {
            return rotation[currentBattle.TurnCount % rotation.Count]();
        }

        protected abstract IList<Func<Spell>> GetRotation();
    }
}