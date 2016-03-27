using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EffectsManager : MonoBehaviour {

    [SerializeField]
    GameObject hitsplatPrefab;
    [SerializeField]
    GameObject minisplatPrefab;
    [SerializeField]
    GameObject bloodsplatPrefab;

    IDictionary<Character, FadeBundle> ColoredFades; //Keep track of Coroutines called on Characters so we can overwrite effects

    struct FadeBundle {
        public Coroutine coroutine;
        public Color pastInitial;
    }

    void Start() {
        ColoredFades = new Dictionary<Character, FadeBundle>();
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

    static readonly Color END_COLOR = Color.white;
    IEnumerator ColoredFade(Character target, Color initial, Color pastInitial, float fadeDuration = 2.0f) {
        Image i = target.Presenter.PortraitView.Image;

        //Set to initial if pastInitial was same color OR if past fade has already completely terminated
        if (pastInitial.Equals(initial) || i.color.Equals(END_COLOR)) {
            i.color = initial;
        } else { //Add in initial when a different color
            i.color += initial;
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
}
