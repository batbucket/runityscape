using UnityEngine;

public class LeftPortraitHolderView : PortraitHolderView {
    public static PortraitHolderView Instance { get; private set; }

    void Awake() { Instance = this; }

    public override PortraitView AddPortrait(string portraitName, Sprite sprite) {
        return AddPortrait(portraitName, sprite, "Left_Portrait");
    }
}

