using Scripts.View.ObjectPool;
using Scripts.View.Portraits;
using System.Collections;
using UnityEngine;

namespace Scripts.View.Effects {

    public class MinisplatEffect : CharacterEffect {
        private static HitsplatView minisplatPrefab = effects.Minisplat;

        private Color color;
        private HitsplatView hitsplat;
        private string text;

        public MinisplatEffect(PortraitView target, Color color, string text) : base(target) {
            this.color = color;
            this.text = text;
        }

        public override void CancelEffect() {
            ObjectPoolManager.Instance.Return(hitsplat);
        }

        protected override IEnumerator EffectRoutine() {
            hitsplat = ObjectPoolManager.Instance.Get(minisplatPrefab);
            hitsplat.Animation(text, color);
            Util.Parent(hitsplat.gameObject, target.EffectsHolder);
            while (hitsplat != null) { }
            isDone = true;
            yield break;
        }
    }
}