using UnityEngine;

public class RightPortraitManager : PortraitManager {
    public override ResourceManager addResource() {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("Right_Resource"));
        Util.parent(g, Util.findChild(gameObject, "Resources"));
        return g.GetComponent<ResourceManager>();
    }
}
