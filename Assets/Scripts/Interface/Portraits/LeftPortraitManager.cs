using UnityEngine;
using System.Collections;
using System;

public class LeftPortraitManager : PortraitManager {
    public override ResourceManager addResource() {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("Left_Resource"));
        Util.parent(g, gameObject);
        return g.GetComponent<ResourceManager>();
    }
}
