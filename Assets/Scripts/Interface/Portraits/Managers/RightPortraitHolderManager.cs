using UnityEngine;

public class RightPortraitHolderManager : PortraitHolderManager {
    public override PortraitManager addPortrait() {
        GameObject g = (GameObject) GameObject.Instantiate(Resources.Load("Right_Portrait"));
        Util.parent(g, gameObject);
        return g.GetComponent<PortraitManager>();
    }
}
