using UnityEngine;

public class LeftPortraitHolderManager : PortraitHolderManager {
    public override PortraitManager AddPortrait(string portraitName, Sprite sprite) {
        return AddPortrait(portraitName, sprite, "Left_Portrait");
    }
}

