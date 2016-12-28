using UnityEngine;
using System.Collections;
using System;

public class BloodsplatEffect : CharacterEffect {
    private static OneShotAnimation bloodsplatPrefab = effects.Bloodsplat;

    private OneShotAnimation bloodsplat;

    public BloodsplatEffect(PortraitView target) : base(target) { }

    public override void CancelEffect() {
        ObjectPoolManager.Instance.Return(bloodsplat);
    }

    protected override IEnumerator EffectRoutine() {
        bloodsplat = ObjectPoolManager.Instance.Get(bloodsplatPrefab);
        Util.Parent(bloodsplat.gameObject, target.gameObject);
        //while (bloodsplat != null) { } //FIXME WTF THIS BREAKS THE CODE
        isDone = true;
        yield break;
    }
}
