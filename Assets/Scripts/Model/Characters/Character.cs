using UnityEngine;
using System;
using System.Collections.Generic;

/**
 * Characters are special Entities with
 * Attributes and Resources
 */
public abstract class Character : Entity {
    public const int CHARGE_PER_TICK = 1;
    public const int CHARGE_CAP_RATIO = 60;

    public CharacterPresenter Presenter { get; set; } //Assigned by PagePresenter

    public string Name { get; set; }
    public int Level { get; set; }

    public IDictionary<AttributeType, Attribute> Attributes { get; private set; }
    public IDictionary<ResourceType, Resource> Resources { get; private set; }
    public SpellFactory Attack { get; set; }
    public IDictionary<Selection, ICollection<SpellFactory>> Selections { get; private set; }

    public Stack<SpellFactory> SpellStack { get; private set; }
    public IList<Spell> Buffs { get; private set; }
    public IList<Spell> RecievedSpells { get; private set; }
    public IList<Spell> CastSpells { get; private set; }

    public Color TextColor { get; private set; }

    public bool Side { get; set; }
    public bool IsTargetable { get; set; }
    public bool IsDisplayable { get; set; }
    public bool IsControllable { get { return this.IsDisplayable && this.IsCharged(); } }
    public ChargeStatus ChargeStatus { get; private set; }

    public PortraitView Portrait { get; set; }

    public Character(Sprite sprite, string name, int level, int strength, int intelligence, int dexterity, int vitality, Color textColor, bool isDisplayable) : base(sprite) {
        this.Name = name;
        this.Level = level;
        this.Attributes = new SortedDictionary<AttributeType, Attribute>() {
            {AttributeType.STRENGTH, new Attribute(strength) },
            {AttributeType.INTELLIGENCE, new Attribute(intelligence) },
            {AttributeType.DEXTERITY, new Attribute(dexterity) },
            {AttributeType.VITALITY, new Attribute(vitality) }
        };

        this.Resources = new SortedDictionary<ResourceType, Resource>() {
            {ResourceType.HEALTH, new Resource(vitality * 10) },
            {ResourceType.CHARGE, new Resource(100) },
        };

        this.Attack = new Attack();
        this.Selections = new SortedDictionary<Selection, ICollection<SpellFactory>>() {
            { Selection.SPELL, new HashSet<SpellFactory>() },
            { Selection.ACT, new HashSet<SpellFactory>() },
            { Selection.ITEM, new Inventory() },
            { Selection.MERCY, new HashSet<SpellFactory>() },
        };
        this.TextColor = textColor;
        this.IsTargetable = true;
        this.IsDisplayable = isDisplayable;
        Resources[ResourceType.CHARGE].ClearFalse();

        SpellStack = new Stack<SpellFactory>();
        Buffs = new List<Spell>();
        RecievedSpells = new List<Spell>();
        CastSpells = new List<Spell>();
    }

    public void AddAttribute(AttributeType attributeType, int initial) {
        this.Attributes[attributeType] = new Attribute(initial);
    }

    public void AddResource(ResourceType resourceType, int initial) {
        this.Resources[resourceType] = new Resource(initial);
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

    public bool AddToAttribute(AttributeType attributeType, bool value, int amount, bool hasMinisplat = false) {
        if (HasAttribute(attributeType) && amount != 0) {
            if (hasMinisplat) {
                Game.Instance.Effect.CreateMinisplat(AttributeType.SplatDisplay(amount), AttributeType.DetermineColor(attributeType, amount), this);
                Game.Instance.Effect.FadeEffect(this, AttributeType.DetermineColor(attributeType, amount));
            }
            Attribute attribute = Attributes[attributeType];
            if (!value) {
                attribute.False += amount;
            } else {
                attribute.True += amount;
            }
            return true;
        }
        return false;
    }

    public bool AddToResource(ResourceType resourceType, bool value, int amount, bool hasHitsplat = false) {
        if (HasResource(resourceType)) {
            if (hasHitsplat && amount != 0) {
                Game.Instance.Effect.CreateHitsplat(resourceType.SplatFunction(amount, GetResourceCount(resourceType, true)), ResourceType.DetermineColor(resourceType, amount), this);
                Game.Instance.Effect.FadeEffect(this, ResourceType.DetermineColor(resourceType, amount));
            }
            Resource resource = Resources[resourceType];
            if (!value) {
                resource.False += amount;
            } else {
                resource.True += amount;
            }
            return true;
        }
        return false;
    }

    public int GetResourceCount(ResourceType resourceType, bool value) {
        if (HasResource(resourceType)) {
            Resource resource;
            Resources.TryGetValue(resourceType, out resource);
            Debug.Assert(resource != null);
            if (!value) {
                return resource.False;
            } else {
                return resource.True;
            }
        } else {
            return 0;
        }
    }

    public int GetAttributeCount(AttributeType attributeType, bool value) {
        if (HasAttribute(attributeType)) {
            Attribute attribute;
            Attributes.TryGetValue(attributeType, out attribute);
            Debug.Assert(attribute != null);
            if (!value) {
                return attribute.False;
            } else {
                return attribute.True;
            }
        } else {
            return 0;
        }
    }

    public void Charge() {
        Debug.Assert(HasResource(ResourceType.CHARGE));
        Resources[ResourceType.CHARGE].False += CHARGE_PER_TICK;
    }

    public void Discharge() {
        Debug.Assert(HasResource(ResourceType.CHARGE));
        Resources[ResourceType.CHARGE].ClearFalse();
    }

    public bool IsCharged() {
        Debug.Assert(HasResource(ResourceType.CHARGE));
        return Resources[ResourceType.CHARGE].IsMaxed();
    }

    public virtual void Tick() {
        Charge();
        if (!Resources[ResourceType.CHARGE].IsMaxed()) {
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
                    WhileFullCharge();
                    break;
            }
        }
        CalculateSpeed(Game.Instance.MainCharacter);

        for (int i = 0; i < Buffs.Count; i++) {
            Spell s = Buffs[i];
            if (s.IsFinished) {
                Buffs.RemoveAt(i);
            } else {
                s.Tick();
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

    public void CalculateSpeed(Character mainCharacter) {
        CalculateSpeed(this, mainCharacter);
    }

    public void AddToBuffs(Spell spell) {
        Buffs.Add(spell);
    }

    static void CalculateSpeed(Character current, Character main) {
        int chargeNeededToAct = (int)(CHARGE_CAP_RATIO * ((float)(main.GetAttributeCount(AttributeType.DEXTERITY, false))) / current.GetAttributeCount(AttributeType.DEXTERITY, false));
        current.Resources[ResourceType.CHARGE].True = chargeNeededToAct;
    }

    protected abstract void OnFullCharge();
    protected abstract void WhileFullCharge();
    public abstract void Act();
    public virtual void React(Spell spell) {
        Result result = spell.Current.DetermineResult();
        Calculation calc = result.Calculation();
        result.Effect(calc);
        result.SFX(calc);
        Game.Instance.TextBoxHolder.AddTextBoxView(new TextBox(result.CreateText(calc), Color.white, TextEffect.FADE_IN));
    }
    public virtual void Witness(Spell spell) { }
    public abstract void OnStart();
    public abstract bool IsDefeated();
    public abstract void OnDefeat();
    public virtual bool IsKilled() { return GetResourceCount(ResourceType.HEALTH, false) <= 0; }
    public virtual void OnKill() { Game.Instance.PagePresenter.Page.GetCharacters(this.Side).Remove(this); }
    public abstract void OnVictory();
    public abstract void OnBattleEnd();
}
