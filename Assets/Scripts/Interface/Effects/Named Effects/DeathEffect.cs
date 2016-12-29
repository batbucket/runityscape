using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine;

public class DeathEffect : CharacterEffect {
    public DeathEffect(PortraitView target) : base(target) {

    }

    public override void CancelEffect() {
        target.Image.color = Color.white;
    }

    protected override IEnumerator EffectRoutine() {
        Game.Instance.Sound.Play("Boom_6");
        target.Image.color = Color.clear;
        this.isDone = true;
        yield break;
    }
}