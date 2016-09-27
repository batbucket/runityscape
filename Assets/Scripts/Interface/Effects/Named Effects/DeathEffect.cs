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
        Image[] images = target.GetComponentsInChildren<Image>();
        Text[] texts = target.GetComponentsInChildren<Text>();
        Game.Instance.Sound.Play("Boom_6");
        foreach (Image i in images) {
            i.color = Color.white;
        }
        foreach (Text t in texts) {
            t.color = Color.white;
        }

        float timer = 0;
        while ((timer += Time.deltaTime) < 0.5f) {
            foreach (Image i in images) {
                if (i != null) {
                    i.color = Color.Lerp(Color.white, Color.clear, timer / 0.5f);
                }
            }
            foreach (Text t in texts) {
                if (t != null) {
                    t.color = Color.Lerp(Color.white, Color.clear, timer / 0.5f);
                }
            }
            yield return null;
        }
        this.Target.Presenter.Character.IsTargetable = false;
        Game.Instance.PagePresenter.Page.GetCharacters(target.Presenter.Character.Side).Remove(target.Presenter.Character);
    }
}