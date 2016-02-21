using System;
using System.Collections.Generic;
using UnityEngine;

public class RightPortraitHolderView : PortraitHolderView {
    [SerializeField]
    GameObject rightPortraitPrefab;

    void Awake() {
        OnAwake();
    }

    public override void AddPortraits(string[] portraitNames) {
        AddPortraits(portraitNames, rightPortraitPrefab);
    }
}
