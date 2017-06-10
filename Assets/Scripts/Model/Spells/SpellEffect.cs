using Scripts.Model.Characters;
using UnityEngine;

namespace Scripts.Model.Spells {

    public abstract class SpellEffect {
        public readonly SpellEffectType EffectType;
        public readonly int Value;

        public SpellEffect(SpellEffectType effect, int value) {
            this.EffectType = effect;
            this.Value = value;
        }

        public abstract void CauseEffect();
    }
}