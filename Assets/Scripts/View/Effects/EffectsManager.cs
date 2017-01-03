using Scripts.View.ObjectPool;
using UnityEngine;

namespace Scripts.View.Effects {

    /// <summary>
    /// This class is used to register effects
    /// and also is a way to get references to various effect prefabs
    /// </summary>
    public class EffectsManager : MonoBehaviour {
        private static EffectsManager instance;

        [SerializeField]
        private OneShotAnimation bloodsplatPrefab;

        [SerializeField]
        private ExplosionView explosionPrefab;

        [SerializeField]
        private HitsplatView hitsplatPrefab;

        [SerializeField]
        private FadeAnimation lightningPrefab;

        [SerializeField]
        private HitsplatView minisplatPrefab;

        public static EffectsManager Instance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType<EffectsManager>();
                }
                return instance;
            }
        }

        public OneShotAnimation Bloodsplat { get { return bloodsplatPrefab; } }
        public ExplosionView Explosion { get { return explosionPrefab; } }
        public HitsplatView Hitsplat { get { return hitsplatPrefab; } }
        public FadeAnimation Lightning { get { return lightningPrefab; } }
        public HitsplatView Minisplat { get { return minisplatPrefab; } }

        private void Start() {
            ObjectPoolManager.Instance.Register(hitsplatPrefab, 10);
            ObjectPoolManager.Instance.Register(minisplatPrefab, 10);
            ObjectPoolManager.Instance.Register(lightningPrefab, 10);
            ObjectPoolManager.Instance.Register(bloodsplatPrefab, 10);
            ObjectPoolManager.Instance.Register(explosionPrefab, 10);
        }
    }
}