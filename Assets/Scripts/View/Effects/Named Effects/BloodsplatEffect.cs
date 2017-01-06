using Scripts.View.ObjectPool;
using Scripts.View.Portraits;
using System.Collections;

namespace Scripts.View.Effects {

    namespace Scripts.Model {

        public class BloodsplatEffect : CharacterEffect {
            private static OneShotAnimation bloodsplatPrefab = effects.Bloodsplat;

            private OneShotAnimation bloodsplat;

            public BloodsplatEffect(PortraitView target) : base(target) {
            }

            public override void CancelEffect() {
                ObjectPoolManager.Instance.Return(bloodsplat);
            }

            protected override IEnumerator EffectRoutine() {
                bloodsplat = ObjectPoolManager.Instance.Get(bloodsplatPrefab);
                Util.Parent(bloodsplat.gameObject, target.EffectsHolder);
                isDone = true;
                yield break;
            }
        }
    }
}