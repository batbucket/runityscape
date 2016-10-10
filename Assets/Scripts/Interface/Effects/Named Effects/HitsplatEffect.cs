using UnityEngine;
using System.Collections;
using System;

public class HitsplatEffect : CharacterEffect {
    private static HitsplatView hitsplatPrefab = effects.Hitsplat;

    private Color color;
    private string text;
    private HitsplatView hitsplat;

    public HitsplatEffect(PortraitView target, Color color, string text) : base(target) {
        this.color = color;
        this.text = text;
    }

    public override void CancelEffect() {
        ObjectPoolManager.Instance.Return(hitsplat);
    }

    protected override IEnumerator EffectRoutine() {
        hitsplat = ObjectPoolManager.Instance.Get(hitsplatPrefab);
        hitsplat.Animation(text, color);
        Util.Parent(hitsplat.gameObject, target.EffectsHolder);
        //while (hitsplat != null) { }
        _isDone = true;
        yield break;
    }
}
