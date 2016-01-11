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
    protected List<Spell> fightSpells;
    protected List<Spell> actSpells;
    protected Inventory inventory;
    protected List<Spell> mercySpells;
    protected bool side;

    protected bool reachedFullCharge;
    protected readonly Color textColor;

    public Character(Sprite sprite, string name, int level, int strength, int intelligence, int dexterity, int vitality, Color textColor) : base(sprite) {
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
        this.fightSpells = new List<Spell>();
        this.actSpells = new List<Spell>();
        this.inventory = new Inventory();
        this.mercySpells = new List<Spell>();
        this.textColor = textColor;

        getResource(ResourceType.CHARGE).clearFalse();
    }

    public void addAttributes(params Attribute[] attributes) {
        foreach (Attribute attribute in attributes) {
            this.attributes.Add(attribute.getAttributeType(), attribute);
        }
    }

    public void addResources(params Resource[] resources) {
        foreach (Resource resource in resources) {
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

    public virtual bool addToAttribute(AttributeType attributeType, bool value, int amount) {
        if (hasAttribute(attributeType)) {
            Attribute attribute = getAttribute(attributeType);
            if (value) {
                attribute.addTrue(amount);
            } else {
                attribute.addFalse(amount);
            }
            return true;
        }
        return false;
    }

    public virtual bool addToResource(ResourceType resourceType, bool value, int amount) {
        if (hasResource(resourceType)) {
            Resource resource = getResource(resourceType);
            if (value) {
                resource.addTrue(amount);
            } else {
                resource.addFalse(amount);
            }
            return true;
        }
        return false;
    }

    public Resource getResource(ResourceType resourceType) {
        Resource resource = null;
        resources.TryGetValue(resourceType, out resource);
        return resource;
    }

    public List<Spell> getFight() {
        return fightSpells;
    }

    public List<Spell> getAct() {
        return actSpells;
    }

    public Inventory getInventory() {
        return inventory;
    }

    public List<Spell> getMercy() {
        return mercySpells;
    }

    public int getLevel() {
        return level;
    }

    public string getName() {
        return name;
    }

    public void setName(string s) {
        name = s;
    }

    public void charge(int amount) {
        Debug.Assert(getResource(ResourceType.CHARGE) != null);
        getResource(ResourceType.CHARGE).addFalse(amount);
    }

    public bool isCharged() {
        Debug.Assert(getResource(ResourceType.CHARGE) != null);
        return getResource(ResourceType.CHARGE).isMaxed();
    }

    public bool getSide() {
        return side;
    }

    protected void getReactions(Spell spell, Game game) {
        List<Character> characters = game.getAll();
        foreach (Character c in characters) {
            c.react(spell, game);
        }
    }

    public bool useItem(Item item, Game game) {
        if (item.canCast()) {
            inventory.remove(item);
            item.tryCast();
            return true;
        }
        return false;
    }

    public virtual void act(int chargeAmount, Game game) {
        charge(chargeAmount);
        if (isCharged() && !reachedFullCharge) {
            reachedFullCharge = true;
            onFullCharge(game);
        }
        if (!isCharged()) {
            reachedFullCharge = false;
        }
    }

    protected void talk(string text, Game game) {
        game.postText(text, this.textColor);
    }

    protected abstract void onFullCharge(Game game);
    protected abstract void react(Spell spell, Game game);
    public abstract void onStart(Game game);
    public abstract bool isDefeated(Game game);
    public abstract void onDefeat(Game game);
    public abstract bool isKilled(Game game);
    public abstract void onKill(Game game);
    public abstract void onVictory(Game game);
    public abstract void onBattleEnd(Game game);


    public override bool Equals(object obj) {
        // If parameter is null return false.
        if (obj == null) {
            return false;
        }

        // If parameter cannot be cast to Page return false.
        Character c = obj as Character;
        if ((object)c == null) {
            return false;
        }

        // Return true if the fields match:
        return c.getName().Equals(this.getName());
    }

    public override int GetHashCode() {
        return name.GetHashCode();
    }

    public void setSide(bool side) {
        this.side = side;
    }

    public Color getColor() {
        return textColor;
    }
}
