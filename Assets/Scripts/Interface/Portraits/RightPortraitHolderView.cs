using UnityEngine;

public class RightPortraitHolderView : PortraitHolderView {
    public static PortraitHolderView Instance { get; private set; }

    void Awake() { Instance = this; }

    public override PortraitView AddPortrait(string portraitName, Sprite sprite) {
        return AddPortrait(portraitName, sprite, "Right_Portrait");
    }
}
