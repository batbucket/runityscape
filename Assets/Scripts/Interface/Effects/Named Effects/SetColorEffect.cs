using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class SetColorEffect : CharacterEffect {
    private Color initial;
    private bool isAffectAll;

    public SetColorEffect(PortraitView target, Color initial, bool isAffectAll = false) : base(target) {
        this.initial = initial;
        this.isAffectAll = isAffectAll;
    }

    public override void CancelEffect() {
        if (isAffectAll) {
            Util.SetAllColorOfChildren(target.gameObject, Color.white);
        } else {
            target.Image.color = Color.white;
        }
    }

    protected override IEnumerator EffectRoutine() {
        Image[] images;
        Text[] texts;
        if (isAffectAll) {
            images = target.gameObject.GetComponentsInChildren<Image>();
            texts = target.gameObject.GetComponentsInChildren<Text>();
        } else {
            images = new Image[] { target.Image };
            texts = new Text[0];
        }

        foreach (Image i in images) {
            i.color = initial;
        }
        foreach (Text t in texts) {
            t.color = initial;
        }
        this.isDone = true;
        yield break;
    }
}
