using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * Characters are special Entities with
 * Attributes and Resources
 */
public abstract class Character : Entity, Battler {
    protected Dictionary<AttributeType, Attribute> attributes;
    protected Dictionary<ResourceType, Resource> resources;
    protected Dictionary<string, UndoableProcess> fightProcesses;
    protected Dictionary<string, UndoableProcess> actProcesses;
    protected Dictionary<string, UndoableProcess> itemProcesses;
    protected Dictionary<string, UndoableProcess> mercyProcesses;
    protected int level;

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

    public abstract void act(Game game);
    public abstract void charge();
    public abstract void react(UndoableProcess process, Game game);
    public abstract void onStart(Game game);
    public abstract bool isDefeated(Game game);
    public abstract void onDefeat(Game game);
    public abstract bool isKilled(Game game);
    public abstract void onKill(Game game);
    public abstract Attribute getAttribute(AttributeType attribute);
    public abstract Resource getResource(ResourceType resource);
}
