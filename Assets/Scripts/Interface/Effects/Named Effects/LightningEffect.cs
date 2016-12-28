using UnityEngine;
using System.Collections;
using System;

public class LightningEffect : CharacterEffect {
    private static FadeAnimation lightningEffectPrefab = effects.Lightning;

    private FadeAnimation lightningEffect;

    public LightningEffect(PortraitView target) : base(target) { }

    public override void CancelEffect() {
        ObjectPoolManager.Instance.Return(lightningEffect);
    }

    protected override IEnumerator EffectRoutine() {
        this.lightningEffect = ObjectPoolManager.Instance.Get(lightningEffectPrefab);
        Util.Parent(lightningEffect.gameObject, target.gameObject);
        isDone = true;
        yield break;
    }
}
