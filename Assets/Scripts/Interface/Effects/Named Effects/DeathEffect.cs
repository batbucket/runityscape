using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine;

public class DeathEffect : CharacterEffect {
    public DeathEffect(PortraitView target) : base(target) {

    }

    public override void CancelEffect() {

    }

    protected override IEnumerator EffectRoutine() {
        target.gameObject.transform.SetParent(null);
        Game.Instance.Sound.Play("Boom_6");

        float timer = 0;
        while ((timer += Time.deltaTime) < 0.5f) {
            target.Image.color = Color.Lerp(target.Image.color, Color.clear, timer / 0.5f);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        this.Target.Presenter.Character.IsTargetable = false;
        Game.Instance.PagePresenter.Page.GetCharacters(target.Presenter.Character.Side).Remove(target.Presenter.Character);
    }
}