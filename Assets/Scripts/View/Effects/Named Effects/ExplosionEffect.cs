using Scripts.View.ObjectPool;
using Scripts.View.Portraits;
using System.Collections;

namespace Scripts.View.Effects {

    public class ExplosionEffect : CharacterEffect {
        private static ExplosionView explosionPrefab = effects.Explosion;

        private ExplosionView ev;

        public ExplosionEffect(PortraitView target) : base(target) {
        }

        public override void CancelEffect() {
            ObjectPoolManager.Instance.Return(ev);
        }

        protected override IEnumerator EffectRoutine() {
            ev = ObjectPoolManager.Instance.Get(explosionPrefab);
            Util.Parent(ev.gameObject, target.EffectsHolder);
            ev.transform.localPosition = target.Image.transform.localPosition;
            ev.transform.parent = null;
            ev.Play();
            while (!ev.IsDone) {
                yield return null;
            }
            isDone = true;
            ObjectPoolManager.Instance.Return(ev);
            yield break;
        }
    }
}