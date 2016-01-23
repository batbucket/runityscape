using UnityEngine;

public abstract class PortraitHolderManager : MonoBehaviour {
    abstract public PortraitManager addPortrait(string portraitName, Sprite sprite);

    protected PortraitManager addPortrait(string portraitName, Sprite sprite, string portraitLocation) {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load(portraitLocation));
        Util.parent(g, gameObject);

        PortraitManager pm = g.GetComponent<PortraitManager>();
        pm.setPortraitName(portraitName);
        pm.setSprite(sprite);
        return pm;
    }
}
