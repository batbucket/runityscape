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
        //target.Image.enabled = false;
        //yield return new WaitForSeconds(1);
        this.Target.Presenter.Character.IsTargetable = false;
        Game.Instance.CurrentPage.GetCharacters(target.Presenter.Character.Side).Remove(target.Presenter.Character);
        this.isDone = true;
        yield break;
    }
}