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
    public IDictionary<Selection, ICollection<Spell>> Selections;

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
        this.Selections = new SortedDictionary<Selection, ICollection<Spell>>()
        {
            { Selection.SPELL, new HashSet<Spell>() },
            { Selection.ACT, new HashSet<Spell>() },
            { Selection.ITEM, new HashSet<Spell>() },
            { Selection.MERCY, new HashSet<Spell>() }

        };
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

    public virtual void Tick(int chargeAmount, Page page) {
        Charge(chargeAmount);
        if (!GetResource(ResourceType.CHARGE).IsMaxed()) {
            ChargeStatus = ChargeStatus.NOT_CHARGED;
        } else {
            switch (ChargeStatus) {
                case ChargeStatus.NOT_CHARGED:
                    ChargeStatus = ChargeStatus.HIT_FULL_CHARGE;
                    break;
                case ChargeStatus.HIT_FULL_CHARGE:
                    OnFullCharge();
                    ChargeStatus = ChargeStatus.FULL_CHARGE;
                    break;
                case ChargeStatus.FULL_CHARGE:
                    break;
            }
        }
        Act(page);
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

    protected abstract void OnFullCharge();
    public abstract void Act(Page page);
    public abstract void React(Spell spell, Page page);
    public abstract void OnStart(Page page);
    public abstract bool IsDefeated();
    public abstract void OnDefeat(Page page);
    public abstract bool IsKilled();
    public abstract void OnKill(Page page);
    public abstract void OnVictory(Page page);
    public abstract void OnBattleEnd(Page page);
}
