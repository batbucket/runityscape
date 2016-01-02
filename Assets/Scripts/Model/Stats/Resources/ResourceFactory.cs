using UnityEngine;
using System.Collections;

public static class ResourceFactory {
    public static Resource createResource(ResourceType resourceType, int value) {
        Resource resource = null;
        switch (resourceType) {
            case ResourceType.HEALTH:
                resource = new Health();
                break;
            case ResourceType.MANA:
                resource = new Mana();
                break;
            case ResourceType.CHARGE:
                resource = new Charge();
                break;
            case ResourceType.SKILL:
                resource = new Skill();
                break;
        }
        resource.setBoth(value);
        return resource;
    }
}
