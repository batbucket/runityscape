using UnityEngine;
using System.Collections;

public static class EffectsFactory {
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
}
