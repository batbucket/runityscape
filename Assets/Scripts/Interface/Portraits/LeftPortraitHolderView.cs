using System;
using System.Collections.Generic;
using UnityEngine;

public class LeftPortraitHolderView : PortraitHolderView {
    [SerializeField]
    GameObject leftPortraitPrefab;

    void Awake() {
        OnAwake();
    }

    public override void AddPortraits(string[] portraitNames) {
        AddPortraits(portraitNames, leftPortraitPrefab);
    }
}

