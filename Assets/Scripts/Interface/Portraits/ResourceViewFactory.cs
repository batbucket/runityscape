using UnityEngine;
using System.Collections;

public static class ResourceViewFactory {
    public static ResourceManager createResource(PortraitManager portraitManager, string resourceName, Color overBarColor, Color underBarColor) {
        ResourceManager resourceManager = portraitManager.addResource();
        resourceManager.setResourceName(resourceName);
        resourceManager.setOverBarColor(overBarColor);
        resourceManager.setUnderBarColor(underBarColor);
        return resourceManager;
    }
}
