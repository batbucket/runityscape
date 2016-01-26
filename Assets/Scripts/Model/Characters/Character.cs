using UnityEngine;
using System;
using System.Collections.Generic;

/**
 * Characters are special Entities with
 * Attributes and Resources
 */
public abstract class Character : Entity {
    public string Name { get; set; }
    public int Level { get; set; }

    public SortedDictionary<AttributeType, Attribute> Attributes { get; private set; }
    public SortedDictionary<ResourceType, Resource> Resources { get; private set; }
    public Spell Attack { get; set; }
    public ICollection<Spell> Fights { get; private set; }
    public ICollection<Spell> Acts { get; private set; }
    public ICollection<Item> Items { get; private set; }
    public ICollection<Spell> Mercies { get; private set; }

    public IList<Spell> RecievedSpells { get; private set; }
    public IList<Spell> CastSpells { get; private set; }

    public Color TextColor { get; private set; }

    public bool Side { get; set; }
    public bool IsTargetable { get; set; }
    public bool Displayable { get; set; }
    public ChargeStatus ChargeStatus { get; private set; }

    public Character(Sprite sprite, string name, int level, int strength, int intelligence, int dexterity, int vitality, Color textColor, bool isDisplayable) : base(sprite) {
        this.Name = name;
        this.Level = level;
        this.Attributes = new SortedDictionary<AttributeType, Attribute>() {
            {AttributeType.STRENGTH, AttributeFactory.CreateAttribute(AttributeType.STRENGTH, strength) },
            {AttributeType.INTELLIGENCE, AttributeFactory.CreateAttribute(AttributeType.INTELLIGENCE, intelligence) },
            {AttributeType.DEXTERITY, AttributeFactory.CreateAttribute(AttributeType.DEXTERITY, dexterity) },
            {AttributeType.VITALITY, AttributeFactory.CreateAttribute(AttributeType.VITALITY, vitality) }
        };

        this.Resources = new SortedDictionary<ResourceType, Resource>() {
            {ResourceType.HEALTH, ResourceFactory.CreateResource(ResourceType.HEALTH, vitality * 10) },
            {ResourceType.CHARGE, ResourceFactory.CreateResource(ResourceType.CHARGE, 100) }
        };

        this.Attack = new Attack();
        this.Fights = new HashSet<Spell>();
        this.Acts = new HashSet<Spell>();
        this.Items = new Inventory();
        this.Mercies = new HashSet<Spell>();
        this.TextColor = textColor;
        this.Displayable = isDisplayable;
        GetResource(ResourceType.CHARGE).clearFalse();

        RecievedSpells = new List<Spell>();
        CastSpells = new List<Spell>();
    }

    public void AddAttributes(params Attribute[] attributes) {
        foreach (Attribute attribute in attributes) {
            this.Attributes.Add(attribute.Type, attribute);
        }
    }

    public void AddResources(params Resource[] resources) {
        foreach (Resource resource in resources) {
            this.Resources.Add(resource.Type, resource);
        }
    }

    public Attribute GetAttribute(AttributeType attributeType) {
        Attribute attribute = null;
        Attributes.TryGetValue(attributeType, out attribute);
        Debug.Assert(attribute != null);
        return attribute;
    }

    public bool HasAttribute(AttributeType attributeType) {
        Attribute attribute = null;
        Attributes.TryGetValue(attributeType, out attribute);
        return attribute != null;
    }

    public bool HasResource(ResourceType resourceType) {
        Resource resource = null;
        Resources.TryGetValue(resourceType, out resource);
        return resource != null;
    }

    public virtual bool AddToAttribute(AttributeType attributeType, bool value, int amount) {
        if (HasAttribute(attributeType)) {
            Attribute attribute = GetAttribute(attributeType);
            if (value) {
                attribute.True += amount;
            } else {
                attribute.False += amount;
            }
            return true;
        }
        return false;
    }

    public virtual bool AddToResource(ResourceType resourceType, bool value, int amount) {
        if (HasResource(resourceType)) {
            Resource resource = GetResource(resourceType);
            if (value) {
                resource.True += amount;
            } else {
                resource.False += amount;
            }
            return true;
        }
        return false;
    }

    public Resource GetResource(ResourceType resourceType) {
        Resource resource = null;
        Resources.TryGetValue(resourceType, out resource);
        return resource;
    }

    public void Charge(int amount) {
        Debug.Assert(GetResource(ResourceType.CHARGE) != null);
        GetResource(ResourceType.CHARGE).False += amount;
    }

    public bool IsCharged() {
        Debug.Assert(GetResource(ResourceType.CHARGE) != null);
        return GetResource(ResourceType.CHARGE).IsMaxed();
    }

    protected void GetReactions(Spell spell, Game game) {
        List<Character> characters = game.GetAll();
        foreach (Character c in characters) {
            c.React(spell, game);
        }
    }

    public bool UseItem(Item item, Character target) {
        if (item.IsCastable(this, target)) {
            Items.Remove(item);
            item.TryCast(this, target);
            return true;
        }
        return false;
    }

    public virtual void Act(int chargeAmount, Game game) {
        Charge(chargeAmount);

        if (!GetResource(ResourceType.CHARGE).IsMaxed()) {
            ChargeStatus = ChargeStatus.NOT_CHARGED;
        } else {
            switch (ChargeStatus) {
                case ChargeStatus.NOT_CHARGED:
                    ChargeStatus = ChargeStatus.HIT_FULL_CHARGE;
                    break;
                case ChargeStatus.HIT_FULL_CHARGE:
                    OnFullCharge(game);
                    ChargeStatus = ChargeStatus.FULL_CHARGE;
                    break;
                case ChargeStatus.FULL_CHARGE:
                    break;
            }
        }
    }

    protected void Talk(string text, Game game) {
        game.PostText(text, this.TextColor);
    }

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
        return this.Name.Equals(c.Name);
    }

    public override int GetHashCode() {
        return Name.GetHashCode();
    }

    public void AddToRecievedSpellHistory(Spell spell) {
        RecievedSpells.Add(spell);
    }

    public void AddToCastSpellHistory(Spell spell) {
        CastSpells.Add(spell);
    }

    protected abstract void OnFullCharge(Game game);
    protected abstract void React(Spell spell, Game game);
    public abstract void OnStart(Game game);
    public abstract bool IsDefeated(Game game);
    public abstract void OnDefeat(Game game);
    public abstract bool IsKilled(Game game);
    public abstract void OnKill(Game game);
    public abstract void OnVictory(Game game);
    public abstract void OnBattleEnd(Game game);
}
