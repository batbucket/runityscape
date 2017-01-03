using Scripts.View.ObjectPool;

namespace Scripts.View.Effects {

    public class OneShotAnimation : PooledBehaviour {

        public override void Reset() {
        }

        private void Kill() {
            ObjectPoolManager.Instance.Return(this);
        }
    }
}