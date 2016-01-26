using UnityEngine;

public class RightPortraitHolderManager : PortraitHolderManager {
    public override PortraitManager AddPortrait(string portraitName, Sprite sprite) {
        return AddPortrait(portraitName, sprite, "Right_Portrait");
    }
}
