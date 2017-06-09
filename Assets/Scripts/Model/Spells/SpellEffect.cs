using Scripts.Model.Characters;
using UnityEngine;

namespace Scripts.Model.Spells {
    public struct SplatDetails {
        public readonly Sprite Sprite;
        public readonly Color Color;
        public readonly string Text;

        public SplatDetails(Color color, string text, Sprite sprite = null) {
            this.Color = color;
            this.Text = text;
            this.Sprite = sprite;
        }
    }

    public abstract class SpellEffect {
        public readonly Characters.Stats Target;
        public readonly SpellEffectType EffectType;
        public readonly int Value;

        public SpellEffect(SpellEffectType effect, Characters.Stats target, int value) {
            this.EffectType = effect;
            this.Target = target;
            this.Value = value;
        }

        public abstract SplatDetails SplatDetails {
            get;
        }

        public abstract void CauseEffect();
    }
}