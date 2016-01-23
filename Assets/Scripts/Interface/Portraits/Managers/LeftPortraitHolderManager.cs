using UnityEngine;

public class LeftPortraitHolderManager : PortraitHolderManager{
    public override PortraitManager addPortrait(string portraitName, Sprite sprite) {
        return addPortrait(portraitName, sprite, "Left_Portrait");
    }
}

