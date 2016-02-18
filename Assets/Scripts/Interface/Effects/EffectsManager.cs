using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EffectsManager : MonoBehaviour {
    public static EffectsManager Instance { get; private set; }
    IDictionary<Character, Coroutine> ColoredFades; //Keep track of Coroutines called on Characters so we can overwrite effects

    void Start() {
        Instance = this;
        ColoredFades = new Dictionary<Character, Coroutine>();
    }

    public static void CreateHitsplat(string text, Color color, Character target) {
        GameObject hitsplat = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Hitsplat"));
        hitsplat.GetComponent<HitsplatView>().GrowAndFade(text, color);
        Util.Parent(hitsplat, target.Presenter.PortraitView.gameObject);
    }

    public static void CreateHitsplat(int text, Color color, Character target) {
        CreateHitsplat("" + text, color, target);
    }

    public static void CreateBloodsplat(Character target) {
        GameObject bloodsplat = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Bloodsplat"));
        Util.Parent(bloodsplat, target.Presenter.PortraitView.gameObject);
    }

    //Calling this will override any FadeEffects on the target's image
    public void RedFadeEffect(Character target) {
        if (ColoredFades.ContainsKey(target)) {
            StopCoroutine(ColoredFades[target]); //Stop coroutine
            target.Presenter.PortraitView.Image.color = Color.white; //Reset color to normal
        }
        ColoredFades[target] = StartCoroutine(ColoredFade(target, Color.red, 2.0f));
    }

    IEnumerator ColoredFade(Character target, Color initial, float fadeDuration) {
        Image i = target.Presenter.PortraitView.Image;
        Color original = i.color;
        i.color = initial;
        float time = 0;

        while (!i.color.Equals(original)) {
            time += Time.deltaTime / fadeDuration;
            i.color = Color.Lerp(i.color, original, time);
            yield return null;
        }
        ColoredFades.Remove(target);
        yield break;
    }
}
