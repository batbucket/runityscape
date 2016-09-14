using UnityEngine;
using System.Collections;
using System;

public class BloodsplatEffect : CharacterEffect {
    private static OneShotAnimation bloodsplatPrefab = effects.Bloodsplat;

    private OneShotAnimation bloodsplat;

    public BloodsplatEffect(PortraitView target) : base(target) { }

    public override void CancelEffect() {
        if (bloodsplat != null) {
            GameObject.Destroy(bloodsplat.gameObject);
        }
    }

    protected override IEnumerator EffectRoutine() {
        bloodsplat = GameObject.Instantiate<OneShotAnimation>(bloodsplatPrefab);
        Util.Parent(bloodsplat.gameObject, target.gameObject);
        //while (bloodsplat != null) { } //FIXME WTF THIS BREAKS THE CODE
        _isDone = true;
        yield break;
    }
}
