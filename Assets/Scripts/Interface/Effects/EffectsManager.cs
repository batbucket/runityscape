using UnityEngine;
using System.Collections;

public class EffectsManager : MonoBehaviour {
    private static EffectsManager _instance;
    public static EffectsManager Instance {
        get {
            return _instance;
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

    private void Awake() {
        _instance = this;
    }
}
