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

    public string Name;
    string suffix;
    public string Suffix { set { suffix = value; } }
    public string DisplayName { get { return string.Format("{0}{1}", Name, string.IsNullOrEmpty(suffix) ? "" : string.Format(" {0}", suffix)); } }
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

    public bool IsTargetable {
        get {
            return State == CharacterState.NORMAL || State == CharacterState.DEFEAT;
        }
    }
    private bool isPlayerControllable;
    public bool IsCommandable { get { return this.isPlayerControllable && this.IsCharged; } }
    public ChargeStatus ChargeStatus { get; private set; }
    public bool IsCharged {
        get {
            return IsCharging && HasResource(ResourceType.CHARGE) && (GetResourceCount(ResourceType.CHARGE, false) == GetResourceCount(ResourceType.CHARGE, true));
        }
    }

    public float BattleTimer;

    public CharacterState State {
        get {
            return state;
        }
        set {
            state = value;
        }
    }
    CharacterState state;

    protected string checkText;
    public string CheckText { get { return checkText; } }

    public bool IsShowingBarCounts;

    public Displays Displays {
        set {
            this.Name = value.Name;
            this.SpriteLoc = value.Loc;
            this.checkText = value.Check;
            this.TextColor = value.Color;
        }
    }

    public Character(bool isControllable, Inventory items, Displays displays, Attributes attributes) : base(displays.Loc) {
        this.Name = displays.Name;
        this.Level = new NamedAttribute.Level();
        Level.False = attributes.Lvl;

        this.Attributes = new SortedDictionary<AttributeType, Attribute>() {
            {AttributeType.LEVEL, Level },
            {AttributeType.STRENGTH, new NamedAttribute.Strength(attributes.Str) },
            {AttributeType.INTELLIGENCE, new NamedAttribute.Intelligence(attributes.Int) },
            {AttributeType.AGILITY, new NamedAttribute.Agility(attributes.Agi) },
            {AttributeType.VITALITY, new NamedAttribute.Vitality(attributes.Vit) }
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

        this.TextColor = displays.Color;
        this.isPlayerControllable = isControllable;

        SpellStack = new Stack<SpellFactory>();
        Buffs = new List<Spell>();
        RecievedSpells = new List<Spell>();
        CastSpells = new List<Spell>();

        PerkType.Character[] characterPerkTypes = (PerkType.Character[])Enum.GetValues(typeof(PerkType.Character));

        PerkType.React[] reactPerkTypes = (PerkType.React[])Enum.GetValues(typeof(PerkType.React));

        CalculateResources();
        FillResources();
        IsCharging = true;
        state = CharacterState.NORMAL;
        this.checkText = displays.Check;
    }

    public void AddAttribute(Attribute attribute) {
        this.Attributes.Add(attribute.Type, attribute);
    }

    public void AddResource(Resource resource) {
        resource.Calculate();
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

    public void Tick() {
        CalculateResources();
        OnTick();
    }

    protected virtual void OnTick() { }

    public void BattleTick(Character main) {
        UpdateState();
        BattleTimer += Time.deltaTime;
        if (main != null) {
            CalculateChargeRequirement(main);
        }
        if (State == CharacterState.NORMAL) {
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

    public TextBox Talk(params string[] s) {
        AvatarBox a = null;
        string sel = s.PickRandom(1).First();
        if (Side) {
            a = new RightBox(SpriteLoc, sel, TextColor);
        } else {
            a = new RightBox(SpriteLoc, sel, TextColor);
        }
        return a;
    }

    public TextBox Emote(string s) {
        return new TextBox(string.Format(s, this.DisplayName));
    }

    public void UpdateState() {
        if (IsDefeated()) {
            OnDefeat();
        } else if (IsKilled()) {
            OnKill();
        }
    }

    protected virtual bool IsDefeated() {
        return State == CharacterState.NORMAL && GetResourceCount(ResourceType.HEALTH, false) <= 0;
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
        return this.GetType().ToString().GetHashCode();
    }

    protected static void CalculateChargeRequirement(Character current, Character main, float chargeCapRatio) {
        int chargeNeededToAct = Math.Max(1, (int)(chargeCapRatio * ((float)(main.GetAttributeCount(AttributeType.AGILITY, false))) / current.GetAttributeCount(AttributeType.AGILITY, false)));
        current.AddToResource(ResourceType.CHARGE, true, -current.GetResourceCount(ResourceType.CHARGE, true));
        current.AddToResource(ResourceType.CHARGE, true, chargeNeededToAct);
    }

    public const float CHARGE_CAP_RATIO = 2f;
    private void CalculateChargeRequirement(Character main) {
        CalculateChargeRequirement(this, main, CHARGE_CAP_RATIO);
    }

    public void AddToBuffs(Spell spell) {
        Buffs.Add(spell);
    }

    protected virtual void OnFullCharge() {
    }

    protected virtual void WhileFullCharge() { }

    protected abstract void Act();

    public virtual void React(Spell spell) {

    }

    public virtual void Witness(Spell spell) {

    }

    public virtual void OnBattleStart() {

    }

    public virtual void OnDefeat() {
        this.state = CharacterState.DEFEAT;
        Game.Instance.TextBoxes.AddTextBox(
            new TextBox(
                string.Format("{0} sustained <color=red>mortal damage</color>.", DisplayName)
            ));
        AddToResource(ResourceType.HEALTH, false, 1, false);
        (new Defeated()).Cast(this, this);
    }

    public virtual void OnKill() {
        this.state = CharacterState.KILLED;
        Game.Instance.TextBoxes.AddTextBox(
            new TextBox(
                string.Format("{0} was <color=red>slain</color>.", DisplayName)
                ));

        Presenter.PortraitView.AddEffect(new DeathEffect(this.Presenter.PortraitView));
        Presenter.PortraitView.AddEffect(new ExplosionEffect(this.Presenter.PortraitView));
        CancelBuffs();
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

    public void CancelBuffs() {
        for (int i = 0; i < Buffs.Count; i++) {
            Buffs[i].Cancel();
        }
        Buffs.Clear();
    }
}
