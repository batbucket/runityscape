using UnityEngine;

public abstract class PortraitHolderManager : MonoBehaviour {

    abstract public PortraitManager AddPortrait(string portraitName, Sprite sprite);

    protected PortraitManager AddPortrait(string portraitName, Sprite sprite, string portraitLocation) {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load(portraitLocation));
        Util.Parent(g, gameObject);

        PortraitManager pm = g.GetComponent<PortraitManager>();
        pm.SetPortraitName(portraitName);
        pm.SetSprite(sprite);
        return pm;
    }
}
