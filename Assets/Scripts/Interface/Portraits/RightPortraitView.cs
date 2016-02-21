using System;
using UnityEngine;

public class RightPortraitView : PortraitView {
    public const int X_OFFSET = -100;

    public override void SetResources(ResourceType[] resourceTypes) {
        SetResources(resourceTypes, "Right_Resource");
    }
}
