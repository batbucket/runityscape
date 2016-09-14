using UnityEngine;
using System.Collections;
using System;

public class LightningEffect : CharacterEffect {
    private static FadeAnimation lightningEffectPrefab = effects.Lightning;

    private FadeAnimation lightningEffect;

    public LightningEffect(PortraitView target) : base(target) { }

    public override void CancelEffect() {
        if (lightningEffect != null) {
            GameObject.Destroy(lightningEffect.gameObject);
        }
    }

    protected override IEnumerator EffectRoutine() {
        this.lightningEffect = GameObject.Instantiate<FadeAnimation>(lightningEffectPrefab);
        Util.Parent(lightningEffect.gameObject, target.gameObject);
        while (lightningEffect != null) { }
        _isDone = true;
        yield break;
    }
}
