using System;
using System.Collections.Generic;
using UnityEngine;

public class RightPortraitHolderView : PortraitHolderView {
    public static PortraitHolderView Instance { get; private set; }

    void Awake() {
        OnAwake();
        Instance = this;
    }

    public override void AddPortraits(string[] portraitNames) {
        AddPortraits(portraitNames, "Right_Portrait");
    }
}
