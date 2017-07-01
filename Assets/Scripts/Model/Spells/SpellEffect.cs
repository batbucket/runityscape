using Scripts.Model.Characters;
using UnityEngine;

namespace Scripts.Model.Spells {

    public abstract class SpellEffect {
        public readonly int Value;

        public SpellEffect(int value) {
            this.Value = value;
        }

        public abstract void CauseEffect();
    }
}