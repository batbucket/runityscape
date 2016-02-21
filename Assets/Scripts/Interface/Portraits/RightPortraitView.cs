using System;
using UnityEngine;

public class RightPortraitView : PortraitView {
    [SerializeField]
    GameObject resourcePrefab;

    public override void SetResources(ResourceType[] resourceTypes) {
        SetResources(resourceTypes, resourcePrefab);
    }
}
