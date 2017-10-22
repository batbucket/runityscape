using Scripts.View.ObjectPool;

namespace Scripts.View.Effects {

    /// <summary>
    /// Animation that plays once
    /// </summary>
    /// <seealso cref="Scripts.View.ObjectPool.PooledBehaviour" />
    public class OneShotAnimation : PooledBehaviour {

        /// <summary>
        /// Reset the state of this MonoBehavior.
        /// </summary>
        public override void Reset() {
        }

        /// <summary>
        /// Kills this instance.
        /// </summary>
        private void Kill() {
            ObjectPoolManager.Instance.Return(this);
        }
    }
}