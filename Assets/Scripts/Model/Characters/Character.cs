using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/**
 * Characters are special Entities with
 * Attributes and Resources
 */
public abstract class Character : Entity {
    public const int CHARGE_MULTIPLIER = 35;
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

    public ICollection<Perk.CharacterPerk> CharacterPerks { get; private set; }
    public ICollection<Perk.SpellPerk> SpellPerks { get; private set; }

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
        Resources[ResourceType.CHARGE].ClearFalse();

        SpellStack = new Stack<SpellFactory>();
        Buffs = new List<Spell>();
        RecievedSpells = new List<Spell>();
        CastSpells = new List<Spell>();

        CharacterPerks = new List<Perk.CharacterPerk>();
        SpellPerks = new List<Perk.SpellPerk>();
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
        AddToResource(ResourceType.CHARGE, false, Time.deltaTime * CHARGE_MULTIPLIER);
    }

    public void Discharge() {
        AddToResource(ResourceType.CHARGE, false, -GetResourceCount(ResourceType.CHARGE, true));
    }

    public bool IsCharged() {
        return HasResource(ResourceType.CHARGE) && (GetResourceCount(ResourceType.CHARGE, false) == GetResourceCount(ResourceType.CHARGE, true));
    }

    public virtual void Tick() {
        InvokeCharacterPerks(Perk.CharacterPerk.PerkType.TICK);
        Charge();
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

    public void InvokeCharacterPerks(Perk.CharacterPerk.PerkType type, Character other = null) {
        foreach (Perk.CharacterPerk p in CharacterPerks) {
            if (p.Type == type) {
                p.Invoke(this, other);
            }
        }
    }

    public void InvokeSpellPerks(Perk.SpellPerk.PerkType type, Spell s, Result r, Calculation c) {
        foreach (Perk.SpellPerk p in SpellPerks) {
            if (p.Type == type) {
                p.Invoke(s, r, c);
            }
        }
    }

    static void CalculateSpeed(Character current, Character main) {
        int chargeNeededToAct = (int)(CHARGE_CAP_RATIO * ((float)(main.GetAttributeCount(AttributeType.DEXTERITY, false))) / current.GetAttributeCount(AttributeType.DEXTERITY, false));
        current.AddToResource(ResourceType.CHARGE, true, -current.GetResourceCount(ResourceType.CHARGE, true));
        current.AddToResource(ResourceType.CHARGE, true, chargeNeededToAct);
    }

    protected virtual void OnFullCharge() {
        InvokeCharacterPerks(Perk.CharacterPerk.PerkType.ON_FULL_CHARGE);
    }

    protected abstract void WhileFullCharge();

    public abstract void Act();

    public virtual void React(Spell spell) {
        Result result = spell.Current.DetermineResult();
        Calculation calc = result.Calculation();
        InvokeSpellPerks(Perk.SpellPerk.PerkType.RECIEVE_SPELL, spell, result, calc);
        result.Effect(calc);
        result.SFX(calc);
        Game.Instance.TextBoxHolder.AddTextBoxView(new TextBox(result.CreateText(calc), Color.white, TextEffect.FADE_IN));
    }
    public virtual void Witness(Spell spell) { }

    public virtual void OnBattleStart() {
        InvokeCharacterPerks(Perk.CharacterPerk.PerkType.BATTLE_START);
    }

    public virtual bool IsDefeated() { return isDefeated || GetResourceCount(ResourceType.HEALTH, false) <= 0; }

    bool isDefeated = false;
    public virtual void OnDefeat() {
        if (isDefeated) {
            return;
        }
        Character defeater = RecievedSpells[RecievedSpells.Count - 1].Caster;
        Game.Instance.TextBoxHolder.AddTextBoxView(
            new TextBox(
                string.Format("* {0} was defeated by {1}.", Name, defeater.Name),
                Color.white, TextEffect.FADE_IN));
        Game.Instance.Effect.ShakeEffect(this, 1f, 0.05f);
        Game.Instance.Effect.StopFadeEffect(this);
        Presenter.PortraitView.Image.color = new Color(1, 0.8f, 0.8f, 0.5f);
        Util.SetTextAlpha(Presenter.PortraitView.PortraitText, 0.5f);
        InvokeCharacterPerks(Perk.CharacterPerk.PerkType.SELF_DEFEAT);
        AddToResource(ResourceType.HEALTH, false, 1, false);
        isDefeated = true;

        foreach (ResourceType res in ResourceType.ALL) {
            if (res != ResourceType.HEALTH) {
                Resources.Remove(res);
            }
        }
    }

    public virtual bool IsKilled() {
        return isKilled || (isDefeated && GetResourceCount(ResourceType.HEALTH, false) <= 0);
    }

    bool isKilled = false;
    public virtual void OnKill() {
        if (isKilled) {
            return;
        }
        Character killer = RecievedSpells[RecievedSpells.Count - 1].Caster;
        Game.Instance.TextBoxHolder.AddTextBoxView(
            new TextBox(
                string.Format("* {0} was slain by {1}.", Name, killer.Name),
                Color.white, TextEffect.FADE_IN));
        InvokeCharacterPerks(Perk.CharacterPerk.PerkType.SELF_KILLED, killer);
        Game.Instance.Sound.Play("Sounds/Boom_6");
        Game.Instance.Effect.FadeAwayEffect(this, 0.5f, () => Game.Instance.PagePresenter.Page.GetCharacters(this.Side).Remove(this));
        isKilled = true;
    }

    public virtual void OnVictory() {
        InvokeCharacterPerks(Perk.CharacterPerk.PerkType.BATTLE_VICTORY);
    }

    public virtual void OnBattleEnd() {
        InvokeCharacterPerks(Perk.CharacterPerk.PerkType.BATTLE_END);
    }
}
