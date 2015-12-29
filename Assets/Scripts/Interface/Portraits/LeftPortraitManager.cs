using UnityEngine;
using System.Collections;
using System;

public class LeftPortraitManager : PortraitManager {
    public override ResourceManager addResource() {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("Left_Resource"));
        Util.parent(g, Util.findChild(gameObject, "Resources"));
        return g.GetComponent<ResourceManager>();
    }
}
