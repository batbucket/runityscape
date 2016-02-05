using UnityEngine;

public abstract class PortraitHolderView : MonoBehaviour {

    abstract public PortraitView AddPortrait(string portraitName, Sprite sprite);

    protected PortraitView AddPortrait(string portraitName, Sprite sprite, string portraitLocation) {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load(portraitLocation));
        Util.Parent(g, gameObject);

        PortraitView pm = g.GetComponent<PortraitView>();
        pm.SetPortraitName(portraitName);
        pm.SetSprite(sprite);
        return pm;
    }
}
