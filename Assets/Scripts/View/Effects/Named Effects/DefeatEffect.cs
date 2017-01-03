using Scripts.View.Portraits;
using System.Collections;
using UnityEngine;

namespace Scripts.View.Effects {

    public class DefeatEffect : CharacterEffect {

        public DefeatEffect(PortraitView target) : base(target) {
        }

        public override void CancelEffect() {
            target.Image.color = Color.white;
        }

        protected override IEnumerator EffectRoutine() {
            target.Image.color = new Color(100f / 255f, 60f / 255f, 60f / 255f);
            this.isDone = true;
            yield break;
        }
    }
}