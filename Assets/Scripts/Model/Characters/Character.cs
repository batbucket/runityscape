using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

/**
 * Characters are special Entities with
 * Attributes and Resources
 */
public abstract class Character : Entity, IReactable {
    public const int CHARGE_MULTIPLIER = 1;

    public CharacterPresenter Presenter { get; set; } //Assigned by PagePresenter

    string _name;
    string _suffix;
    public string Name { set { _name = value; } get { return _name; } }
    public string Suffix { set { _suffix = value; } }
    public string DisplayName { get { return string.Format("{0}{1}", _name, string.IsNullOrEmpty(_suffix) ? "" : string.Format(" {0}", _suffix)); } }
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

    public IDictionary<PerkType.Character, IList<InvokePerk>> CharacterPerks;
    public IDictionary<PerkType.React, IList<ReactPerk>> ReactPerks;

    public CharacterState State {
        get {
            return _state;
        }
        set {
            _state = value;
        }
    }
    CharacterState _state;

    string _checkText;
    public string CheckText { get { return _checkText; } }

    public Character(string spriteLoc, string name, int level, int strength, int intelligence, int dexterity, int vitality, Color textColor, bool isDisplayable, string checkText = "") : base(spriteLoc) {
        this._name = name;
        this.Level = level;

        this.Attributes = new SortedDictionary<AttributeType, Attribute>() {
            {AttributeType.STRENGTH, new NamedAttribute.Strength(strength) },
            {AttributeType.INTELLIGENCE, new NamedAttribute.Intelligence(intelligence) },
            {AttributeType.DEXTERITY, new NamedAttribute.Dexterity(dexterity) },
            {AttributeType.VITALITY, new NamedAttribute.Vitality(vitality) }
        };

        this.Resources = new SortedDictionary<ResourceType, Resource>() {
            {ResourceType.HEALTH, new NamedResource.Health((NamedAttribute.Vitality)Attributes[AttributeType.VITALITY]) },
            {ResourceType.CHARGE, new NamedResource.Charge() },
        };

        this.Attack = new Attack();
        Inventory inventory = new Inventory();
        this.Selections = new SortedDictionary<Selection, ICollection<SpellFactory>>() {
            { Selection.SPELL, new HashSet<SpellFactory>() },
            { Selection.ACT, new HashSet<SpellFactory>() },
            { Selection.ITEM, inventory },
            { Selection.MERCY, new HashSet<SpellFactory>() },
            { Selection.EQUIP, new Equipment(inventory) }
        };
        this.TextColor = textColor;
        this.IsTargetable = true;
        this.IsDisplayable = isDisplayable;

        SpellStack = new Stack<SpellFactory>();
        Buffs = new List<Spell>();
        RecievedSpells = new List<Spell>();
        CastSpells = new List<Spell>();


        PerkType.Character[] characterPerkTypes = (PerkType.Character[])Enum.GetValues(typeof(PerkType.Character));

        PerkType.React[] reactPerkTypes = (PerkType.React[])Enum.GetValues(typeof(PerkType.React));

        CalculateResources();
        FillResources();
        IsCharging = true;
        _state = CharacterState.ALIVE;
        _checkText = checkText;
    }

    public void AddAttribute(Attribute attribute) {
        this.Attributes.Add(attribute.Type, attribute);
    }

    public void AddResource(Resource resource) {
        this.Resources.Add(resource.Type, resource);
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

    public bool AddToAttribute(AttributeType attributeType, bool value, float amount, bool hasMinisplat = false) {
        if (HasAttribute(attributeType) && amount != 0) {
            Attribute attribute = Attributes[attributeType];
            if (!value) {
                attribute.False += amount;
            } else {
                attribute.True += (int)amount;
            }
            CalculateResources();
            return true;
        }
        return false;
    }

    public bool AddToResource(ResourceType resourceType, bool value, float amount, bool hasHitsplat = false) {
        if (HasResource(resourceType)) {
            Resource resource = Resources[resourceType];
            if (!value) {
                resource.False += amount;
            } else {
                resource.True += (int)amount;
            }
            return true;
        }
        return false;
    }

    public int GetResourceCount(ResourceType resourceType, bool value) {
        if (HasResource(resourceType)) {
            Resource resource;
            Resources.TryGetValue(resourceType, out resource);
            if (!value) {
                return (int)resource.False;
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
            if (!value) {
                return (int)attribute.False;
            } else {
                return attribute.True;
            }
        } else {
            return 0;
        }
    }

    public void Charge() {
        if (IsCharging) {
            AddToResource(ResourceType.CHARGE, false, Time.deltaTime * CHARGE_MULTIPLIER);
        }
    }

    public void Discharge() {
        AddToResource(ResourceType.CHARGE, false, -GetResourceCount(ResourceType.CHARGE, true));
    }

    public bool IsCharged() {
        return HasResource(ResourceType.CHARGE) && (GetResourceCount(ResourceType.CHARGE, false) == GetResourceCount(ResourceType.CHARGE, true));
    }

    void CalculateResources() {
        foreach (KeyValuePair<ResourceType, Resource> pair in Resources) {
            pair.Value.Calculate();
        }

        //Set Skill cap to be highest skill costing Spell
        if (Resources.ContainsKey(ResourceType.SKILL) && Selections[Selection.SPELL].Count > 0) {
            Resources[ResourceType.SKILL].True = Mathf.Max(2, Selections[Selection.SPELL].Where(s => s.Costs.ContainsKey(ResourceType.SKILL)).Select(s => s.Costs[ResourceType.SKILL]).OrderByDescending(i => i).FirstOrDefault());
        }
    }

    public void FillResources() {
        foreach (KeyValuePair<ResourceType, Resource> pair in Resources) {
            pair.Value.False = pair.Value.True;
        }
    }

    bool isCharging;
    public bool IsCharging {
        get {
            return isCharging;
        }
        set {
            isCharging = value;
            Discharge();
        }
    }

    public virtual void Tick(Character mainCharacter, bool isInCombat) {
        CalculateResources();
        if (isInCombat) {

            foreach (KeyValuePair<ResourceType, Resource> pair in Resources) {
                if (pair.Key != ResourceType.HEALTH) {
                    pair.Value.IsVisible = State == CharacterState.ALIVE;
                }
            }

            if (State == CharacterState.ALIVE) {
                Charge();
            }
            if (!IsCharged()) {
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
            CalculateChargeRequirement(mainCharacter);

            if (IsDefeated()) {
                _state = CharacterState.KILLED;
            } else if (IsKilled()) {
                _state = CharacterState.DEFEAT;
            }

            if (State == CharacterState.KILLED) {
                OnKill();
            } else if (State == CharacterState.DEFEAT) {
                OnDefeat();
            }

            for (int i = 0; i < Buffs.Count; i++) {
                Spell s = Buffs[i];
                s.Tick();
            }

            Act();
        }
    }

    protected virtual bool IsDefeated() {
        return State == CharacterState.DEFEAT && GetResourceCount(ResourceType.HEALTH, false) <= 0;
    }

    protected virtual bool IsKilled() {
        return State == CharacterState.ALIVE && GetResourceCount(ResourceType.HEALTH, false) <= 0;
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
        return this == c;
    }

    public override int GetHashCode() {
        return DisplayName.GetHashCode();
    }

    protected static void CalculateChargeRequirement(Character current, Character main, float chargeCapRatio) {
        int chargeNeededToAct = Math.Max(1, (int)(chargeCapRatio * ((float)(main.GetAttributeCount(AttributeType.DEXTERITY, false))) / current.GetAttributeCount(AttributeType.DEXTERITY, false)));
        current.AddToResource(ResourceType.CHARGE, true, -current.GetResourceCount(ResourceType.CHARGE, true));
        current.AddToResource(ResourceType.CHARGE, true, chargeNeededToAct);
    }

    public abstract void CalculateChargeRequirement(Character main);

    public void AddToBuffs(Spell spell) {
        Buffs.Add(spell);
    }

    protected virtual void OnFullCharge() {
    }

    protected virtual void WhileFullCharge() { }

    public abstract void Act();

    public virtual void React(Spell spell) {

    }

    public virtual void Witness(Spell spell) {

    }

    public virtual void OnBattleStart() {
    }

    bool defeatPosted;
    public virtual void OnDefeat(bool isSilent = false) {
        if (defeatPosted) {
            return;
        }
        defeatPosted = true;
        if (!isSilent) {
            Character defeater = RecievedSpells[RecievedSpells.Count - 1].Caster;
            Game.Instance.TextBoxHolder.AddTextBoxView(
                new TextBox(
                    string.Format("{0} was <color=red>defeated</color> by {1}.", DisplayName, defeater.DisplayName),
                    Color.white, TextEffect.FADE_IN)
                );
        }
        Presenter.PortraitView.Image.color = new Color(1, 0.8f, 0.8f, 0.5f);
        Util.SetTextAlpha(Presenter.PortraitView.PortraitText, 0.5f);
        AddToResource(ResourceType.HEALTH, false, 1, false);
        Discharge();
        Presenter.PortraitView.AddEffect(new DefeatEffect(this.Presenter.PortraitView));
    }

    bool killPosted;
    public virtual void OnKill(bool isSilent = false) {
        if (killPosted) {
            return;
        }
        killPosted = true;
        if (!isSilent) {
            Character killer = RecievedSpells[RecievedSpells.Count - 1].Caster;
            Game.Instance.TextBoxHolder.AddTextBoxView(
                new TextBox(
                    string.Format("{0} was <color=red>slain</color> by {1}.", DisplayName, killer.DisplayName),
                    Color.white, TextEffect.FADE_IN));
        }
        this.IsTargetable = false;
        Buffs.Clear();
        Presenter.PortraitView.ClearEffects();
        Presenter.PortraitView.AddEffect(new DeathEffect(this.Presenter.PortraitView));
    }

    public string AttributeDistribution {
        get {
            List<string> s = new List<string>();
            foreach (KeyValuePair<AttributeType, Attribute> pair in Attributes) {
                s.Add(string.Format("{0} {1}/{2}", pair.Key.ShortName, pair.Value.False, pair.Value.True));
            }
            return string.Format("{0}.", string.Join(", ", s.ToArray()));
        }
    }

    public virtual void OnVictory() {
    }

    public virtual void OnBattleEnd() {
        AddToResource(ResourceType.SKILL, false, -GetResourceCount(ResourceType.SKILL, false));
    }
}
