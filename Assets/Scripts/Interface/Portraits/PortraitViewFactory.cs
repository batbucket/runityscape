using UnityEngine;
using System.Collections;

public static class PortraitViewFactory {
    public static PortraitManager createPortrait(PortraitHolderManager portraitHolderManager, string portraitName, Sprite sprite) {
        PortraitManager portraitManager = portraitHolderManager.addPortrait();
        portraitManager.setPortraitName(portraitName);
        portraitManager.setIconImage(sprite);
        return portraitManager;
    }
}
