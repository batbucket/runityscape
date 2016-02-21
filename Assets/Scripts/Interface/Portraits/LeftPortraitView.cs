using UnityEngine;
using System.Collections;
using System;

public class LeftPortraitView : PortraitView {
    [SerializeField]
    GameObject resourcePrefab;

    public override void SetResources(ResourceType[] resourceTypes) {
        SetResources(resourceTypes, resourcePrefab);
    }
}
