using UnityEngine;

public class RightPortraitHolderManager : PortraitHolderManager {
    public static PortraitHolderManager Instance { get; private set; }

    void Awake() { Instance = this; }

    public override PortraitManager AddPortrait(string portraitName, Sprite sprite) {
        return AddPortrait(portraitName, sprite, "Right_Portrait");
    }
}
