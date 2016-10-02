using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine;

public class DefeatEffect : CharacterEffect {
    public DefeatEffect(PortraitView target) : base(target) {

    }

    public override void CancelEffect() {
        target.Image.color = Color.white;
    }

    protected override IEnumerator EffectRoutine() {
        yield return new WaitForSeconds(.75f);
        target.Image.color = new Color(100f / 255f, 60f / 255f, 60f / 255f);
        target.AddEffect(new ShakeEffect(target, 0.05f, 0.05f));
        yield break;
    }
}
