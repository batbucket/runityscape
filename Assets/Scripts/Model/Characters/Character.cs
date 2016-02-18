using UnityEngine;
using System;
using System.Collections.Generic;

/**
 * Characters are special Entities with
 * Attributes and Resources
 */
public abstract class Character : Entity {
    public const int CHARGE_PER_TICK = 1;
    public const int CHARGE_CAP_RATIO = 120;

    public CharacterPresenter Presenter { get; set; } //Assigned by PagePresenter

    public string Name { get; set; }
    public int Level { get; set; }

    public IDictionary<AttributeType, Attribute> Attributes { get; private set; }
    public IDictionary<ResourceType, Resource> Resources { get; private set; }
    public Spell Attack { get; set; }
    public IDictionary<Selection, ICollection<Spell>> Selections { get; private set; }

    public Stack<Spell> SpellStack { get; private set; } //Spells casted are pushed here by reference.
    public IList<Spell> RecievedSpells { get; private set; } //Spells recieved are added by value.
    public IList<Spell> CastSpells { get; private set; } //Spells cast are added by value.

    public Color TextColor { get; private set; }

    public bool Side { get; set; }
    public bool IsTargetable { get; set; }
    public bool IsDisplayable { get; set; }
    public ChargeStatus ChargeStatus { get; private set; }

    public PortraitView Portrait { get; set; }

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
            {ResourceType.CHARGE, ResourceFactory.CreateResource(ResourceType.CHARGE, 100) },
        };

        this.Attack = new Attack();
        this.Selections = new SortedDictionary<Selection, ICollection<Spell>>() {
            { Selection.SPELL, new HashSet<Spell>() },
            { Selection.ACT, new HashSet<Spell>() },
            { Selection.ITEM, new Inventory() },
            { Selection.MERCY, new HashSet<Spell>() },
        };
        this.TextColor = textColor;
        this.IsTargetable = true;
        this.IsDisplayable = isDisplayable;
        GetResource(ResourceType.CHARGE).clearFalse();

        SpellStack = new Stack<Spell>();
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

    public void Charge() {
        Debug.Assert(GetResource(ResourceType.CHARGE) != null);
        GetResource(ResourceType.CHARGE).False += CHARGE_PER_TICK;
    }

    public bool IsCharged() {
        Debug.Assert(GetResource(ResourceType.CHARGE) != null);
        return GetResource(ResourceType.CHARGE).IsMaxed();
    }

    public virtual void Tick() {
        Charge();
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
        Act();
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

    public void CalculateSpeed(Character mainCharacter) {
        CalculateSpeed(this, mainCharacter);
    }

    static void CalculateSpeed(Character current, Character main) {
        int chargeNeededToAct = (int)(CHARGE_CAP_RATIO * ((float)(main.GetAttribute(AttributeType.DEXTERITY).False)) / current.GetAttribute(AttributeType.DEXTERITY).False);
        current.GetResource(ResourceType.CHARGE).True = chargeNeededToAct;
    }

    public bool IsControllable() {
        return this.IsDisplayable && this.IsCharged();
    }

    protected abstract void OnFullCharge();
    public abstract void Act();
    public abstract void React(Spell spell);
    public abstract void OnStart();
    public abstract bool IsDefeated();
    public abstract void OnDefeat();
    public abstract bool IsKilled();
    public abstract void OnKill();
    public abstract void OnVictory();
    public abstract void OnBattleEnd();
}
