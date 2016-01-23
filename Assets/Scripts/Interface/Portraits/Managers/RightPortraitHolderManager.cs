using UnityEngine;

public class RightPortraitHolderManager : PortraitHolderManager {
    public override PortraitManager addPortrait(string portraitName, Sprite sprite) {
        return addPortrait(portraitName, sprite, "Right_Portrait");
    }
}
