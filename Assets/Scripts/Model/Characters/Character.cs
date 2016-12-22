using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

/**
 * Characters are special Entities with
 * Attributes and Resources
 */
public abstract class Character : Entity, IReactable {
    public static readonly float CHARGE_MULTIPLIER = 60 * Time.deltaTime;

    public CharacterPresenter Presenter { get; set; } //Assigned by PagePresenter

    string _name;
    string _suffix;
    public string Name { set { _name = value; } get { return _name; } }
    public string Suffix { set { _suffix = value; } }
    public string DisplayName { get { return string.Format("{0}{1}", _name, string.IsNullOrEmpty(_suffix) ? "" : string.Format(" {0}", _suffix)); } }
    public NamedAttribute.Level Level;

    public IDictionary<AttributeType, Attribute> Attributes { get; private set; }
    public IDictionary<ResourceType, Resource> Resources { get; private set; }
    public SpellFactory Attack { get; set; }

    public IList<SpellFactory> Spells;
    public IList<SpellFactory> Actions;
    public Inventory Items;
    public IList<SpellFactory> Mercies;
    public Equipment Equipment;

    public Stack<SpellFactory> SpellStack { get; private set; }
    public List<Spell> Buffs { get; private set; }

    public IList<Spell> RecievedSpells { get; private set; }
    public IList<Spell> CastSpells { get; private set; }

    public Color TextColor { get; private set; }

    public bool Side { get; set; }

    private bool isTargetable;
    public bool IsTargetable {
        get {
            return isTargetable;
        }
        set {
            isTargetable = value;
            Discharge();
        }
    }
    public bool IsControllable { get; set; }
    public bool IsActive { get { return this.IsControllable && this.IsCharged; } }
    public ChargeStatus ChargeStatus { get; private set; }
    public bool IsCharged {
        get {
            return HasResource(ResourceType.CHARGE) && (GetResourceCount(ResourceType.CHARGE, false) == GetResourceCount(ResourceType.CHARGE, true));
        }
    }

    public IDictionary<PerkType.Character, IList<InvokePerk>> CharacterPerks;
    public IDictionary<PerkType.React, IList<ReactPerk>> ReactPerks;

    public float BattleTimer;

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

    public bool IsShowingBarCounts;

    public Character(Inventory items, string spriteLoc, string name, int level, int strength, int intelligence, int dexterity, int vitality, Color textColor, bool isDisplayable, string checkText = "") : base(spriteLoc) {
        this._name = name;
        this.Level = new NamedAttribute.Level();
        Level.False = level;

        this.Attributes = new SortedDictionary<AttributeType, Attribute>() {
            {AttributeType.LEVEL, Level },
            {AttributeType.STRENGTH, new NamedAttribute.Strength(strength) },
            {AttributeType.INTELLIGENCE, new NamedAttribute.Intelligence(intelligence) },
            {AttributeType.AGILITY, new NamedAttribute.Agility(dexterity) },
            {AttributeType.VITALITY, new NamedAttribute.Vitality(vitality) }
        };

        this.Resources = new SortedDictionary<ResourceType, Resource>() {
            {ResourceType.HEALTH, new NamedResource.Health((NamedAttribute.Vitality)Attributes[AttributeType.VITALITY]) },
            {ResourceType.CHARGE, new NamedResource.Charge() },
        };

        this.Attack = new Attack();

        Spells = new List<SpellFactory>();
        Actions = new List<SpellFactory>();
        Items = items;
        Mercies = new List<SpellFactory>();
        Equipment = new Equipment(items);

        this.TextColor = textColor;
        this.IsTargetable = true;
        this.IsControllable = isDisplayable;

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

    void CalculateResources() {
        foreach (KeyValuePair<ResourceType, Resource> pair in Resources) {
            pair.Value.Calculate();
        }

        //Set Skill cap to be highest skill costing Spell
        if (Resources.ContainsKey(ResourceType.SKILL) && Spells.Count > 0) {
            Resources[ResourceType.SKILL].True = Mathf.Max(2, Spells.Where(s => s.Costs.ContainsKey(ResourceType.SKILL)).Select(s => s.Costs[ResourceType.SKILL]).OrderByDescending(i => i).FirstOrDefault());
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
        CalculateChargeRequirement(mainCharacter);

        if (isInCombat) {

            BattleTimer += Time.deltaTime;

            foreach (KeyValuePair<ResourceType, Resource> pair in Resources) {
                if (pair.Key != ResourceType.HEALTH) {
                    pair.Value.IsVisible = State == CharacterState.ALIVE;
                }
            }

            if (State == CharacterState.ALIVE) {
                Charge();
            }
            if (!IsCharged) {
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

            Buffs.RemoveAll(s => s.IsTimedOut);
            for (int i = 0; i < Buffs.Count; i++) {
                Spell buff = Buffs[i];
                buff.Tick();
            }

            Act();
        }
    }

    public void UpdateState() {
        if (IsDefeated()) {
            OnDefeat();
        } else if (IsKilled()) {
            OnKill();
        }
    }

    protected virtual bool IsDefeated() {
        return State == CharacterState.ALIVE && GetResourceCount(ResourceType.HEALTH, false) <= 0;
    }

    protected virtual bool IsKilled() {
        return State == CharacterState.DEFEAT && GetResourceCount(ResourceType.HEALTH, false) <= 0;
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
        int chargeNeededToAct = Math.Max(1, (int)(chargeCapRatio * ((float)(main.GetAttributeCount(AttributeType.AGILITY, false))) / current.GetAttributeCount(AttributeType.AGILITY, false)));
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

    public virtual void OnDefeat() {
        if (_state == CharacterState.DEFEAT) {
            return;
        }
        _state = CharacterState.DEFEAT;
        Game.Instance.TextBoxes.AddTextBox(
            new TextBox(
                string.Format("{0} sustained <color=red>mortal damage</color>.", DisplayName),
                Color.white, TextEffect.FADE_IN)
            );
        AddToResource(ResourceType.HEALTH, false, 1, false);
        Discharge();
        Presenter.PortraitView.AddEffect(new DefeatEffect(this.Presenter.PortraitView));
    }

    public virtual void OnKill() {
        if (_state == CharacterState.KILLED) {
            return;
        }
        _state = CharacterState.KILLED;
        Game.Instance.TextBoxes.AddTextBox(
            new TextBox(
                string.Format("{0} was <color=red>slain</color>.", DisplayName),
                Color.white, TextEffect.FADE_IN));
        Presenter.PortraitView.AddEffect(new ExplosionEffect(this.Presenter.PortraitView));
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
