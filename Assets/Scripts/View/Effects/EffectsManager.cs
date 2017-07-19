using Scripts.View.ObjectPool;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.Effects {

    /// <summary>
    /// This class is used to register effects
    /// and also is a way to get references to various effect prefabs
    /// </summary>
    public class EffectsManager : MonoBehaviour {
        private static EffectsManager instance;

        [SerializeField]
        private GameObject foreground;

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

        [SerializeField]
        private ShroudView shroudPrefab;

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
        public GameObject Foreground { get { return foreground; } }
        public ShroudView Shroud { get { return shroudPrefab; } }

        private void Start() {
            ObjectPoolManager.Instance.Register(hitsplatPrefab, 10);
            ObjectPoolManager.Instance.Register(minisplatPrefab, 10);
            ObjectPoolManager.Instance.Register(lightningPrefab, 10);
            ObjectPoolManager.Instance.Register(bloodsplatPrefab, 10);
            ObjectPoolManager.Instance.Register(explosionPrefab, 10);
            ObjectPoolManager.Instance.Register(shroudPrefab, 10);
        }
    }
}