using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * Characters are special Entities with
 * Attributes and Resources
 */
public abstract class Character : Entity {
    protected string name;
    protected int level;
    protected SortedDictionary<AttributeType, Attribute> attributes;
    protected SortedDictionary<ResourceType, Resource> resources;
    protected List<string> fightSpells;
    protected List<string> actSpells;
    protected List<Item> items;
    protected List<string> mercySpells;

    public Character(Sprite sprite, string name, int level, int strength, int intelligence, int dexterity, int vitality) : base(sprite) {
        this.name = name;
        this.level = level;
        this.attributes = new SortedDictionary<AttributeType, Attribute>() {
            {AttributeType.STRENGTH, AttributeFactory.createAttribute(AttributeType.STRENGTH, strength) },
            {AttributeType.INTELLIGENCE, AttributeFactory.createAttribute(AttributeType.INTELLIGENCE, intelligence) },
            {AttributeType.DEXTERITY, AttributeFactory.createAttribute(AttributeType.DEXTERITY, dexterity) },
            {AttributeType.VITALITY, AttributeFactory.createAttribute(AttributeType.VITALITY, vitality) }
        };

        this.resources = new SortedDictionary<ResourceType, Resource>() {
            {ResourceType.HEALTH, ResourceFactory.createResource(ResourceType.HEALTH, vitality * 10) },
            {ResourceType.CHARGE, ResourceFactory.createResource(ResourceType.CHARGE, 100) }
        };
        this.fightSpells = new List<string>();
        this.actSpells = new List<string>();
        this.items = new List<Item>();
        this.mercySpells = new List<string>();

        getResource(ResourceType.CHARGE).clearFalse();
    }

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

    public Attribute getAttribute(AttributeType attributeType) {
        Attribute attribute = null;
        attributes.TryGetValue(attributeType, out attribute);
        Debug.Assert(attribute != null);
        return attribute;
    }

    public SortedDictionary<AttributeType, Attribute> getAttributes() {
        return attributes;
    }

    public SortedDictionary<ResourceType, Resource> getResources() {
        return resources;
    }

    public bool hasAttribute(AttributeType attributeType) {
        Attribute attribute = null;
        attributes.TryGetValue(attributeType, out attribute);
        return attribute != null;
    }

    public bool hasResource(ResourceType resourceType) {
        Resource resource = null;
        resources.TryGetValue(resourceType, out resource);
        return resource != null;
    }

    public void addToAttribute(AttributeType attributeType, bool value, int amount) {
        if (hasAttribute(attributeType)) {
            Attribute attribute = getAttribute(attributeType);
            if (value) {
                attribute.addTrue(amount);
            } else {
                attribute.addFalse(amount);
            }
        }
    }

    public void addToResource(ResourceType resourceType, bool value, int amount) {
        if (hasResource(resourceType)) {
            Resource resource = getResource(resourceType);
            if (value) {
                resource.addTrue(amount);
            } else {
                resource.addFalse(amount);
            }
        }
    }

    public Resource getResource(ResourceType resourceType) {
        Resource resource = null;
        resources.TryGetValue(resourceType, out resource);
        return resource;
    }

    public List<string> getFight() {
        return fightSpells;
    }

    public List<string> getAct() {
        return actSpells;
    }

    public List<Item> getItem() {
        return items;
    }

    public List<string> getMercy() {
        return mercySpells;
    }

    public int getLevel() {
        return level;
    }

    public string getName() {
        return name;
    }

    public void charge(int amount) {
        Debug.Assert(getResource(ResourceType.CHARGE) != null);
        getResource(ResourceType.CHARGE).addFalse(amount);
    }

    public bool isCharged() {
        Debug.Assert(getResource(ResourceType.CHARGE) != null);
        return getResource(ResourceType.CHARGE).isMaxed();
    }

    public abstract void act(int chargeAmount, Game game);
    public abstract void react(Spell spell, Game game);
    public abstract void onStart(Game game);
    public abstract bool isDefeated(Game game);
    public abstract void onDefeat(Game game);
    public abstract bool isKilled(Game game);
    public abstract void onKill(Game game);
    public abstract void onVictory(Game game);
    public abstract void onBattleEnd(Game game);
}
