using UnityEngine;
using System.Collections;

public class EffectsManager : MonoBehaviour {
    private static EffectsManager instance;
    public static EffectsManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<EffectsManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private HitsplatView hitsplatPrefab;
    public HitsplatView Hitsplat { get { return hitsplatPrefab; } }

    [SerializeField]
    private HitsplatView minisplatPrefab;
    public HitsplatView Minisplat { get { return minisplatPrefab; } }

    [SerializeField]
    private FadeAnimation lightningPrefab;
    public FadeAnimation Lightning { get { return lightningPrefab; } }

    [SerializeField]
    private OneShotAnimation bloodsplatPrefab;
    public OneShotAnimation Bloodsplat { get { return bloodsplatPrefab; } }

    private void Start() {
        ObjectPoolManager.Instance.Register(hitsplatPrefab, 10);
        ObjectPoolManager.Instance.Register(minisplatPrefab, 10);
        ObjectPoolManager.Instance.Register(lightningPrefab, 10);
        ObjectPoolManager.Instance.Register(bloodsplatPrefab, 10);
    }
}
