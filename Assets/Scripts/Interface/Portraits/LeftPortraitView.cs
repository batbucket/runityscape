using UnityEngine;
using System.Collections;
using System;

public class LeftPortraitView : PortraitView {

    public override void AddResources(ResourceType[] resourceTypes) {
        AddResources(resourceTypes, "Left_Resource");
    }
}
