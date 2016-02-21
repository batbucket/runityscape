using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EffectsManager : MonoBehaviour {

    [SerializeField]
    GameObject hitsplatPrefab;
    [SerializeField]
    GameObject bloodsplatPrefab;

    IDictionary<Character, Coroutine> ColoredFades; //Keep track of Coroutines called on Characters so we can overwrite effects

    void Start() {
        ColoredFades = new Dictionary<Character, Coroutine>();
    }

    public void CreateHitsplat(string text, Color color, Character target) {
        GameObject hitsplat = GameObject.Instantiate<GameObject>(hitsplatPrefab);
        hitsplat.GetComponent<HitsplatView>().GrowAndFade(text, color);
        Util.Parent(hitsplat, target.Presenter.PortraitView.gameObject);
    }

    public void CreateHitsplat(int text, Color color, Character target) {
        CreateHitsplat("" + text, color, target);
    }

    public void CreateBloodsplat(Character target) {
        GameObject bloodsplat = GameObject.Instantiate<GameObject>(bloodsplatPrefab);
        Util.Parent(bloodsplat, target.Presenter.PortraitView.gameObject);
    }

    //Calling this will override any FadeEffects on the target's image
    public void RedFadeEffect(Character target) {
        target.Presenter.PortraitView.Image.color = Color.white; //Reset color to normal
        if (ColoredFades.ContainsKey(target)) {
            StopCoroutine(ColoredFades[target]); //Stop coroutine
        }
        ColoredFades[target] = StartCoroutine(ColoredFade(target, Color.red, 2.0f));
    }

    IEnumerator ColoredFade(Character target, Color initial, float fadeDuration) {
        Image i = target.Presenter.PortraitView.Image;
        Color original = i.color;
        i.color = initial;
        float time = 0;

        while (!i.color.Equals(original)) {
            if (i == null) {
                yield break;
            }
            time += Time.deltaTime / fadeDuration;
            i.color = Color.Lerp(i.color, original, time);
            yield return null;
        }
        ColoredFades.Remove(target);
        yield break;
    }
}
