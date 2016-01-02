using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/**
 * Characters are special Entities with
 * Attributes and Resources
 */
public abstract class Character : Entity, Battler {
    Dictionary<AttributeType, Attribute> attributes;
    Dictionary<ResourceType, Resource> resources;
    int level;

    public void addAttributes(params Attribute[] attributes) {
        foreach(Attribute attribute in attributes) {
            this.attributes.Add(attribute.getAttributeType(), attribute);
        }
    }

    public void addResources(params Resource[] resources) {
        foreach(Resource resource in resources) {
            this.resources.Add(resource.getResourceType(), resource);
        }
    }

    public void setLevel(int level) {
        this.level = level;
    }

    public abstract void act(TextBoxHolderManager textBoxHolderManager);
    public abstract void react(BattleProcess process);
    public abstract void onStart();
    public abstract bool isDefeated();
    public abstract void onDefeat();
    public abstract bool isKilled();
    public abstract void onKill();
    public abstract int getResource(ResourceType resource);
}
