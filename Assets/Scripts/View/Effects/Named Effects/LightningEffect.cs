using Scripts.View.ObjectPool;
using Scripts.View.Portraits;
using System.Collections;

namespace Scripts.View.Effects {

    public class LightningEffect : CharacterEffect {
        private static FadeAnimation lightningEffectPrefab = effects.Lightning;

        private FadeAnimation lightningEffect;

        public LightningEffect(PortraitView target) : base(target) {
        }

        public override void CancelEffect() {
            ObjectPoolManager.Instance.Return(lightningEffect);
        }

        protected override IEnumerator EffectRoutine() {
            this.lightningEffect = ObjectPoolManager.Instance.Get(lightningEffectPrefab);
            Util.Parent(lightningEffect.gameObject, target.gameObject);
            isDone = true;
            yield break;
        }
    }
}