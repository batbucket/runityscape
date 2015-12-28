using UnityEngine;
using System.Collections;
using System;

public class RightPortraitManager : PortraitManager {
    public override ResourceManager addResource() {
        GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("Right_Resource"));
        Util.parent(g, gameObject);
        return g.GetComponent<ResourceManager>();
    }
}
