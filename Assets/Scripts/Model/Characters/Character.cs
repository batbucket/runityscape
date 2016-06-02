using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

/**
 * Characters are special Entities with
 * Attributes and Resources
 */
public abstract class Character : Entity, IReactable {
    public const int CHARGE_MULTIPLIER = 35;
    public const int CHARGE_CAP_RATIO = 60;

    public CharacterPresenter Presenter { get; set; } //Assigned by PagePresenter

    string _name;
    public string Name { get { return Util.Color(_name, TextColor); } set { _name = value; } }
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

    public Character(string spriteLoc, string name, int level, int strength, int intelligence, int dexterity, int vitality, Color textColor, bool isDisplayable) : base(spriteLoc) {
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

        this.Attack = new CounterAttack();
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
            if (hasMinisplat) {
                Game.Instance.Effect.CreateMinisplat(AttributeType.SplatDisplay((int)amount), AttributeType.DetermineColor(attributeType, (int)amount), this);
                Game.Instance.Effect.FadeEffect(this, AttributeType.DetermineColor(attributeType, (int)amount));
            }
            Attribute attribute = Attributes[attributeType];
            if (!value) {
                attribute.False += amount;
            } else {
                attribute.True += (int)amount;
            }
            return true;
        }
        return false;
    }

    public bool AddToResource(ResourceType resourceType, bool value, float amount, bool hasHitsplat = false) {
        if (HasResource(resourceType)) {
            if (hasHitsplat && amount != 0) {
                Game.Instance.Effect.CreateHitsplat(resourceType.SplatFunction((int)amount, GetResourceCount(resourceType, true)), ResourceType.DetermineColor(resourceType, (int)amount), this);
                Game.Instance.Effect.FadeEffect(this, ResourceType.DetermineColor(resourceType, (int)amount));
                if (resourceType == ResourceType.HEALTH && !value && amount < 0) { //Shake on negative health additions, intensity based on hit
                    Game.Instance.Effect.ShakeEffect(this, -amount / GetResourceCount(ResourceType.HEALTH, true));
                }
            }
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
            Debug.Assert(resource != null);
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
            Debug.Assert(attribute != null);
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

    public bool IsCharging { set; get; }

    public virtual void Tick(bool isInCombat) {
        CalculateResources();
        if (isInCombat) {

            foreach (KeyValuePair<ResourceType, Resource> pair in Resources) {
                if (pair.Key != ResourceType.HEALTH) {
                    pair.Value.IsVisible = !IsDefeated();
                }
            }

            if (isDefeated && GetResourceCount(ResourceType.HEALTH, false) > 1) {
                isDefeated = false;
            }

            if (!IsDefeated()) {
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
            CalculateChargeRequirement(Game.Instance.MainCharacter);

            if (IsDefeated()) { OnDefeat(); }
            if (IsKilled()) { OnKill(); }

            for (int i = 0; i < Buffs.Count; i++) {
                Spell s = Buffs[i];
                s.Tick();
            }

            Act();
        }
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
        return Name.GetHashCode();
    }

    public void CalculateChargeRequirement(Character mainCharacter) {
        CalculateChargeRequirement(this, mainCharacter);
    }

    public void AddToBuffs(Spell spell) {
        Buffs.Add(spell);
    }

    static void CalculateChargeRequirement(Character current, Character main) {
        int chargeNeededToAct = Math.Max(1, (int)(CHARGE_CAP_RATIO * ((float)(main.GetAttributeCount(AttributeType.DEXTERITY, false))) / current.GetAttributeCount(AttributeType.DEXTERITY, false)));
        current.AddToResource(ResourceType.CHARGE, true, -current.GetResourceCount(ResourceType.CHARGE, true));
        current.AddToResource(ResourceType.CHARGE, true, chargeNeededToAct);
    }

    protected virtual void OnFullCharge() {
    }

    protected abstract void WhileFullCharge();

    public abstract void Act();

    public virtual void React(Spell spell, Result res, Calculation calc) {

    }

    public virtual void Witness(Spell spell, Result res, Calculation calc) {

    }

    public virtual void OnBattleStart() {
    }

    public virtual bool IsDefeated() {
        return (isDefeated && GetResourceCount(ResourceType.HEALTH, false) == 1) || GetResourceCount(ResourceType.HEALTH, false) <= 0;
    }

    bool isDefeated = false;
    public virtual void OnDefeat(bool isSilent = false) {
        if (isDefeated) {
            return;
        }
        if (!isSilent) {
            Character defeater = RecievedSpells[RecievedSpells.Count - 1].Caster;
            Game.Instance.TextBoxHolder.AddTextBoxView(
                new TextBox(
                    string.Format("{0} was defeated by {1}.", Name, defeater.Name),
                    Color.white, TextEffect.FADE_IN)
                );
            Game.Instance.Effect.CreateHitsplat("DEFEAT", Color.white, this);
        }

        Game.Instance.Effect.StopFadeEffect(this);
        Game.Instance.Effect.ShakeEffect(this, 1f, 0.05f);
        Presenter.PortraitView.Image.color = new Color(1, 0.8f, 0.8f, 0.5f);
        Util.SetTextAlpha(Presenter.PortraitView.PortraitText, 0.5f);
        AddToResource(ResourceType.HEALTH, false, 1, false);
        Discharge();
        isDefeated = true;
    }

    public virtual bool IsKilled() {
        return isKilled || (IsDefeated() && GetResourceCount(ResourceType.HEALTH, false) <= 0);
    }

    bool isKilled = false;
    public virtual void OnKill(bool isSilent = false) {
        if (isKilled) {
            return;
        }
        if (!isSilent) {
            Character killer = RecievedSpells[RecievedSpells.Count - 1].Caster;
            Game.Instance.TextBoxHolder.AddTextBoxView(
                new TextBox(
                    string.Format("{0} was slain by {1}.", Name, killer.Name),
                    Color.white, TextEffect.FADE_IN));
            Game.Instance.Sound.Play("Sounds/Boom_6");
            Game.Instance.Effect.CreateHitsplat("DEATH", Color.white, this);
        }
        Game.Instance.Effect.CharacterDeath(this, 0.5f, () => Game.Instance.PagePresenter.Page.GetCharacters(this.Side).Remove(this));
        isKilled = true;
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
    }
}
