using UnityEngine;
using System.Collections;
using System;

public class LeftPortraitView : PortraitView {

    public override void SetResources(ResourceType[] resourceTypes) {
        SetResources(resourceTypes, "Left_Resource");
    }
}
