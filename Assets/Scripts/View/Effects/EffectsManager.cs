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

        /// <summary>
        /// The foreground
        /// </summary>
        [SerializeField]
        private GameObject foreground;

        /// <summary>
        /// The bloodsplat prefab
        /// </summary>
        [SerializeField]
        private OneShotAnimation bloodsplatPrefab;

        /// <summary>
        /// The explosion prefab
        /// </summary>
        [SerializeField]
        private ExplosionView explosionPrefab;

        /// <summary>
        /// The hitsplat prefab
        /// </summary>
        [SerializeField]
        private HitsplatView hitsplatPrefab;

        /// <summary>
        /// The lightning prefab
        /// </summary>
        [SerializeField]
        private FadeAnimation lightningPrefab;

        /// <summary>
        /// The minisplat prefab
        /// </summary>
        [SerializeField]
        private HitsplatView minisplatPrefab;

        /// <summary>
        /// The shroud prefab
        /// </summary>
        [SerializeField]
        private ShroudView shroudPrefab;

        /// <summary>
        /// The steam burst prefab
        /// </summary>
        [SerializeField]
        private ExplosionView steamBurstPrefab;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static EffectsManager Instance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType<EffectsManager>();
                }
                return instance;
            }
        }

        /// <summary>
        /// Gets the bloodsplat.
        /// </summary>
        /// <value>
        /// The bloodsplat.
        /// </value>
        public OneShotAnimation Bloodsplat { get { return bloodsplatPrefab; } }

        /// <summary>
        /// Gets the explosion.
        /// </summary>
        /// <value>
        /// The explosion.
        /// </value>
        public ExplosionView Explosion { get { return explosionPrefab; } }

        /// <summary>
        /// Gets the hitsplat.
        /// </summary>
        /// <value>
        /// The hitsplat.
        /// </value>
        public HitsplatView Hitsplat { get { return hitsplatPrefab; } }

        /// <summary>
        /// Gets the lightning.
        /// </summary>
        /// <value>
        /// The lightning.
        /// </value>
        public FadeAnimation Lightning { get { return lightningPrefab; } }

        /// <summary>
        /// Gets the minisplat.
        /// </summary>
        /// <value>
        /// The minisplat.
        /// </value>
        public HitsplatView Minisplat { get { return minisplatPrefab; } }

        /// <summary>
        /// Gets the foreground.
        /// </summary>
        /// <value>
        /// The foreground.
        /// </value>
        public GameObject Foreground { get { return foreground; } }

        /// <summary>
        /// Gets the shroud.
        /// </summary>
        /// <value>
        /// The shroud.
        /// </value>
        public ShroudView Shroud { get { return shroudPrefab; } }

        /// <summary>
        /// Gets the steam burst.
        /// </summary>
        /// <value>
        /// The steam burst.
        /// </value>
        public ExplosionView SteamBurst { get { return steamBurstPrefab; } }

        private void Start() {
            ObjectPoolManager.Instance.Register(hitsplatPrefab);
            ObjectPoolManager.Instance.Register(minisplatPrefab);
            ObjectPoolManager.Instance.Register(lightningPrefab);
            ObjectPoolManager.Instance.Register(bloodsplatPrefab);
            ObjectPoolManager.Instance.Register(explosionPrefab);
            ObjectPoolManager.Instance.Register(shroudPrefab);
            ObjectPoolManager.Instance.Register(steamBurstPrefab);
        }
    }
}