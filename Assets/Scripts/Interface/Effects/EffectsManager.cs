using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class EffectsManager : MonoBehaviour {

    [SerializeField]
    GameObject hitsplatPrefab;
    [SerializeField]
    GameObject minisplatPrefab;
    [SerializeField]
    GameObject bloodsplatPrefab;

    struct FadeBundle {
        public Coroutine coroutine;
        public Color pastInitial;
    }
    IDictionary<Character, FadeBundle> ColoredFades; //Keep track of Coroutines called on Characters so we can overwrite effects

    struct ShakeBundle {
        public Coroutine coroutine;
        public Vector2 originalPos;
    }
    IDictionary<Character, ShakeBundle> Shakes;

    void Start() {
        ColoredFades = new Dictionary<Character, FadeBundle>();
        Shakes = new Dictionary<Character, ShakeBundle>();
    }

    const float BASE_INTENSITY = 50;
    const float DURATION = 1.0f;
    public void ShakeEffect(Character target, float startIntensityPercentage, float endIntensityPercentage = 0) {
        if (Shakes.ContainsKey(target) && Shakes[target].coroutine != null) {
            StopCoroutine(Shakes[target].coroutine); //Stop coroutine
            target.Presenter.PortraitView.IconTransform.localPosition = Shakes[target].originalPos;
        }
        Vector2 originalPos = target.Presenter.PortraitView.IconTransform.localPosition;
        Shakes[target] = new ShakeBundle() { coroutine = StartCoroutine(ShakeIcon(target, startIntensityPercentage * BASE_INTENSITY, endIntensityPercentage * BASE_INTENSITY)), originalPos = originalPos };
    }

    IEnumerator ShakeIcon(Character target, float startIntensity, float endIntensity = 0) {
        RectTransform icon = target.Presenter.PortraitView.IconTransform;
        Vector2 originalPos = icon.localPosition;
        float currentIntensity = startIntensity;
        while (target.Presenter.PortraitView != null && currentIntensity > 0) {
            Mathf.SmoothDamp(currentIntensity, endIntensity, ref currentIntensity, DURATION);
            icon.localPosition = new Vector2(originalPos.x + Mathf.Sin(UnityEngine.Random.Range(0, 5)) * currentIntensity, originalPos.y + Mathf.Sin(UnityEngine.Random.Range(0, 5)) * currentIntensity);
            yield return null;
        }
        if (target.Presenter.PortraitView != null) {
            icon.localPosition = originalPos;
        }
        yield break;
    }

    public void CreateMinisplat(string text, Color color, Character target) {
        GameObject minisplat = GameObject.Instantiate<GameObject>(minisplatPrefab);
        minisplat.GetComponent<HitsplatView>().Animation(text, color);
        Util.Parent(minisplat, target.Presenter.PortraitView.Hitsplats);
    }

    public void CreateHitsplat(string text, Color color, Character target) {
        GameObject hitsplat = GameObject.Instantiate<GameObject>(hitsplatPrefab);
        hitsplat.GetComponent<HitsplatView>().Animation(text, color);
        Util.Parent(hitsplat, target.Presenter.PortraitView.Hitsplats);
    }

    public void CreateHitsplat(int text, Color color, Character target) {
        CreateHitsplat("" + text, color, target);
    }

    public void CreateBloodsplat(Character target) {
        GameObject bloodsplat = GameObject.Instantiate<GameObject>(bloodsplatPrefab);
        Util.Parent(bloodsplat, target.Presenter.PortraitView.gameObject);
    }

    //Calling this will override any FadeEffects on the target's image
    public void FadeEffect(Character target, Color color) {
        if (ColoredFades.ContainsKey(target) && ColoredFades[target].coroutine != null) {
            StopCoroutine(ColoredFades[target].coroutine); //Stop coroutine
        }
        ColoredFades[target] = new FadeBundle() { coroutine = StartCoroutine(ColoredFade(target, color, !ColoredFades.ContainsKey(target) ? Color.white : ColoredFades[target].pastInitial)), pastInitial = color };
    }

    public void StopFadeEffect(Character target) {
        if (ColoredFades.ContainsKey(target) && ColoredFades[target].coroutine != null) {
            StopCoroutine(ColoredFades[target].coroutine); //Stop coroutine
        }
    }

    static readonly Color END_COLOR = Color.white;
    IEnumerator ColoredFade(Character target, Color initial, Color pastInitial, float fadeDuration = 2.0f) {
        Image i = target.Presenter.PortraitView.Image;

        //Set to initial if pastInitial was same color OR if past fade has already completely terminated
        if (pastInitial.Equals(initial) || i.color.Equals(END_COLOR)) {
            i.color = initial;
        } else { //Add in initial when a different color
            i.color = (i.color / 2) + (initial / 2);
        }
        float time = 0;

        while (!i.color.Equals(END_COLOR)) {
            if (i == null) {
                yield break;
            }
            time += Time.deltaTime / fadeDuration;
            i.color = Color.Lerp(i.color, END_COLOR, time);
            yield return null;
        }
        ColoredFades.Remove(target);
        yield break;
    }

    public void CharacterDeath(Character target, float duration, Action endCall = null) {
        target.Sprite = null;
        StartCoroutine(ColorTransition(target.Presenter.PortraitView.gameObject, duration, Color.white, Color.clear, endCall));
    }

    public void FadeIn(Character target, float duration, Action endCall = null) {
        StartCoroutine(ColorTransition(target.Presenter.PortraitView.gameObject, duration, Color.clear, Color.white, endCall));
    }

    public void ColorShift(GameObject target, float duration, Color initial, Color destination, Action endCall = null) {
        StartCoroutine(ColorTransition(target, duration, initial, destination, endCall));
    }

    public void Fade(GameObject target, float duration, float initial, float destination, Action endCall = null) {
        StartCoroutine(FadeEffect(target, duration, initial, destination, endCall));
    }

    IEnumerator ColorTransition(GameObject target, float duration, Color initial, Color destination, Action endCall = null) {
        Image[] images = target.gameObject.GetComponentsInChildren<Image>();
        Text[] texts = target.gameObject.GetComponentsInChildren<Text>();
        float timer = duration;

        foreach (Image i in images) {
            i.color = Color.white;
        }
        foreach (Text t in texts) {
            t.color = Color.white;
        }

        while ((timer -= Time.deltaTime) > 0) {
            Color lerp = Color.Lerp(initial, destination, (duration - timer) / duration);
            foreach (Image i in images) {
                i.color = lerp;
            }
            foreach (Text t in texts) {
                t.color = lerp;
            }
            yield return null;
        }
        if (endCall != null) {
            endCall.Invoke();
        }
        yield break;
    }

    IEnumerator FadeEffect(GameObject target, float duration, float initial, float destination, Action endCall = null) {
        Image[] images = target.gameObject.GetComponentsInChildren<Image>();
        Text[] texts = target.gameObject.GetComponentsInChildren<Text>();
        Outline[] outlines = target.gameObject.GetComponentsInChildren<Outline>();
        float timer = duration;

        while ((timer -= Time.deltaTime) > 0) {
            float alpha = Mathf.Lerp(initial, destination, (duration - timer) / duration);
            foreach (Image i in images) {
                Util.SetImageAlpha(i, alpha);
            }
            foreach (Text t in texts) {
                Util.SetTextAlpha(t, alpha);
            }
            foreach (Outline o in outlines) {
                Color c = o.effectColor;
                c.a = alpha;
                o.effectColor = c;
            }
            yield return null;
        }
        foreach (Image i in images) {
            Util.SetImageAlpha(i, destination);
        }
        foreach (Text t in texts) {
            Util.SetTextAlpha(t, destination);
        }
        foreach (Outline o in outlines) {
            Color c = o.effectColor;
            c.a = destination;
            o.effectColor = c;
        }
        if (endCall != null) {
            endCall.Invoke();
        }
        yield break;
    }
}
