using Scripts.Presenter;
using Scripts.View.Portraits;
using System.Collections;
using UnityEngine;

namespace Scripts.View.Effects {

    public class DeathEffect : CharacterEffect {

        public DeathEffect(PortraitView target) : base(target) {
        }

        public override void CancelEffect() {
            target.Image.color = Color.white;
        }

        protected override IEnumerator EffectRoutine() {
            Game.Instance.Sound.PlaySound("Boom_6");
            target.Image.color = Color.clear;
            this.isDone = true;
            yield break;
        }
    }
}