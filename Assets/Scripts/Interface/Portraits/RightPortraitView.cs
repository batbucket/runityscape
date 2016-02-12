using System;
using UnityEngine;

public class RightPortraitView : PortraitView {
    public const int X_OFFSET = -100;

    public override void AddResources(ResourceType[] resourceTypes) {
        AddResources(resourceTypes, "Right_Resource");
    }
}
