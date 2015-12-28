using UnityEngine;

public class LeftPortraitHolderManager : PortraitHolderManager{
    public override PortraitManager addPortrait() {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("Left_Portrait"));
        Util.parent(g, gameObject);
        return g.GetComponent<PortraitManager>();
    }
}

