using UnityEngine;

public class LeftPortraitHolderManager : PortraitHolderManager {
    public static PortraitHolderManager Instance { get; private set; }

    void Awake() { Instance = this; }

    public override PortraitManager AddPortrait(string portraitName, Sprite sprite) {
        return AddPortrait(portraitName, sprite, "Left_Portrait");
    }
}

