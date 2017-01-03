using Scripts.Model.Interfaces;
using Scripts.Model.Items;
using Scripts.Model.Perks;
using Scripts.Model.Spells;
using Scripts.Model.Spells.Named;
using Scripts.Model.Stats.Attributes;
using Scripts.Model.Stats.Resources;
using Scripts.Model.TextBoxes;
using Scripts.Presenter;
using Scripts.View.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Model.Characters {

    /// <summary>
    /// Characters are special entities with
    /// Resources and Attributes, as well as numerous SpellFactories.
    ///
    /// They can participate in battles.
    /// </summary>
    public abstract class Character : Entity, IReactable {
        public const float CHARGE_CAP_RATIO = 2f;
        public static readonly float CHARGE_MULTIPLIER = 60 * Time.deltaTime;

        public IList<SpellFactory> Actions;
        public IDictionary<AttributeType, Stats.Attributes.Attribute> Attributes;
        public float BattleTimer;
        public List<Spell> Buffs;
        public IList<Spell> CastSpells;
        public Equipment Equipment;
        public IList<SpellFactory> Flees;
        public Inventory Inventory;
        public bool IsShowingBarCounts;
        public string Name;
        public IList<Spell> RecievedSpells;
        public IDictionary<ResourceType, Resource> Resources;
        public bool Side;
        public IList<SpellFactory> Spells;
        public Stack<SpellFactory> SpellStack;
        public Color TextColor;
        protected string checkText;
        private bool isCharging;
        private bool isPlayerControllable;
        private CharacterState state;
        private string suffix;

        public Character(bool isControllable, Displays displays, StartStats attributes, Items items) : base(displays.Loc) {
            this.Name = displays.Name;

            this.Attack = new Attack();

            Spells = new List<SpellFactory>();
            Actions = new List<SpellFactory>();
            Inventory = new Inventory();
            Flees = new List<SpellFactory>();
            Equipment = new Equipment();
            foreach (Item i in items.Inventory) {
                Inventory.Add(i);
            }
            foreach (KeyValuePair<EquipmentType, EquippableItem> e in items.Equips) {
                if (e.Value == null) {
                }
                Equipment.Add(e.Value);
            }

            this.Attributes = new SortedDictionary<AttributeType, Stats.Attributes.Attribute>();
            AddAttribute(new NamedAttribute.Level(attributes.Lvl));
            AddAttribute(new NamedAttribute.Strength(attributes.Str));
            AddAttribute(new NamedAttribute.Intelligence(attributes.Int));
            AddAttribute(new NamedAttribute.Agility(attributes.Agi));
            AddAttribute(new NamedAttribute.Vitality(attributes.Vit));

            this.Resources = new SortedDictionary<ResourceType, Resource>();
            AddResource(new NamedResource.Health());
            AddResource(new NamedResource.Charge());

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

        public Character(bool isControllable, Displays displays, StartStats attributes) : this(isControllable, displays, attributes, new Items(new Item[0], new EquippableItem[0])) {
        }

        public SpellFactory Attack { get; set; }

        public string AttributeDistribution {
            get {
                List<string> s = new List<string>();
                foreach (KeyValuePair<AttributeType, Stats.Attributes.Attribute> pair in Attributes) {
                    s.Add(string.Format("{0} {1}/{2}", pair.Key.ShortName, pair.Value.False, pair.Value.True));
                }
                return string.Format("{0}.", string.Join(", ", s.ToArray()));
            }
        }

        public ChargeStatus ChargeStatus { get; private set; }
        public string CheckText { get { return checkText; } }
        public string DisplayName { get { return string.Format("{0}{1}", Name, string.IsNullOrEmpty(suffix) ? "" : string.Format(" {0}", suffix)); } }

        public Displays Displays {
            set {
                this.Name = value.Name;
                this.SpriteLoc = value.Loc;
                this.checkText = value.Check;
                this.TextColor = value.Color;
            }
        }

        public bool IsCharged {
            get {
                return IsCharging && HasResource(ResourceType.CHARGE) && (GetResourceCount(ResourceType.CHARGE, false) == GetResourceCount(ResourceType.CHARGE, true));
            }
        }

        public bool IsCharging {
            get {
                return isCharging;
            }
            set {
                isCharging = value;
                Discharge();
            }
        }

        public bool IsCommandable { get { return this.isPlayerControllable && this.IsCharged; } }

        public bool IsTargetable {
            get {
                return State == CharacterState.NORMAL || State == CharacterState.DEFEAT;
            }
        }

        public int Level {
            get {
                return GetAttributeCount(AttributeType.LEVEL, false);
            }
        }

        public CharacterPresenter Presenter { get; set; } //Assigned by PagePresenter

        public CharacterState State {
            get {
                return state;
            }
            set {
                state = value;
            }
        }

        public string Suffix { set { suffix = value; } }

        public void AddAttribute(Stats.Attributes.Attribute attribute) {
            this.Attributes.Add(attribute.Type, attribute);
        }

        public void AddResource(Resource resource) {
            resource.Calculate(GetAttributeCount(resource.Dependent, false));
            this.Resources.Add(resource.Type, resource);
        }

        public bool AddToAttribute(AttributeType attributeType, bool value, float amount, bool hasMinisplat = false) {
            if (HasAttribute(attributeType) && amount != 0) {
                Stats.Attributes.Attribute attribute = Attributes[attributeType];
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

        public void AddToBuffs(Spell spell) {
            Buffs.Add(spell);
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

        public void CancelBuffs() {
            for (int i = 0; i < Buffs.Count; i++) {
                Buffs[i].Cancel();
            }
            Buffs.Clear();
        }

        public void Charge() {
            if (IsCharging) {
                AddToResource(ResourceType.CHARGE, false, Time.deltaTime * CHARGE_MULTIPLIER);
            }
        }

        public void Discharge() {
            AddToResource(ResourceType.CHARGE, false, -GetResourceCount(ResourceType.CHARGE, true));
        }

        public TextBox Emote(string s) {
            return new TextBox(string.Format(s, this.DisplayName));
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

        public void FillResources() {
            foreach (KeyValuePair<ResourceType, Resource> pair in Resources) {
                pair.Value.False = pair.Value.True;
            }
        }

        public int GetAttributeCount(AttributeType attributeType, bool value) {
            if (HasAttribute(attributeType)) {
                Stats.Attributes.Attribute attribute;
                Attributes.TryGetValue(attributeType, out attribute);
                if (!value) {
                    return (int)(attribute.False + Equipment.GetAttributeCount(attributeType));
                } else {
                    return attribute.True;
                }
            } else {
                return 0;
            }
        }

        public override int GetHashCode() {
            return this.GetType().ToString().GetHashCode();
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

        public bool HasAttribute(AttributeType attributeType) {
            Stats.Attributes.Attribute attribute = null;
            Attributes.TryGetValue(attributeType, out attribute);
            return attribute != null;
        }

        public bool HasResource(ResourceType resourceType) {
            Resource resource = null;
            Resources.TryGetValue(resourceType, out resource);
            return resource != null;
        }

        public virtual void OnBattleEnd() {
            AddToResource(ResourceType.SKILL, false, -GetResourceCount(ResourceType.SKILL, false));
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

            Presenter.PortraitView.AddEffect(new DeathEffect(Presenter.PortraitView));
            Presenter.PortraitView.AddEffect(new ExplosionEffect(Presenter.PortraitView));
            CancelBuffs();
        }

        public virtual void OnVictory() {
        }

        public virtual void React(Spell spell) {
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

        public void Tick() {
            CalculateResources();
            OnTick();
        }

        public void UpdateState() {
            if (IsDefeated()) {
                OnDefeat();
            } else if (IsKilled()) {
                OnKill();
            }
        }

        public virtual void Witness(Spell spell) {
        }

        protected static void CalculateChargeRequirement(Character current, Character main, float chargeCapRatio) {
            int chargeNeededToAct = Math.Max(1, (int)(chargeCapRatio * ((float)(main.GetAttributeCount(AttributeType.AGILITY, false))) / current.GetAttributeCount(AttributeType.AGILITY, false)));
            current.AddToResource(ResourceType.CHARGE, true, -current.GetResourceCount(ResourceType.CHARGE, true));
            current.AddToResource(ResourceType.CHARGE, true, chargeNeededToAct);
        }

        protected abstract void Act();

        protected virtual bool IsDefeated() {
            return State == CharacterState.NORMAL && GetResourceCount(ResourceType.HEALTH, false) <= 0;
        }

        protected virtual bool IsKilled() {
            return State == CharacterState.DEFEAT && GetResourceCount(ResourceType.HEALTH, false) <= 0;
        }

        protected virtual void OnFullCharge() {
        }

        protected virtual void OnTick() {
        }

        protected virtual void WhileFullCharge() {
        }

        private void CalculateChargeRequirement(Character main) {
            CalculateChargeRequirement(this, main, CHARGE_CAP_RATIO);
        }

        private void CalculateResources() {
            foreach (KeyValuePair<ResourceType, Resource> pair in Resources) {
                if (pair.Value.Dependent != null) {
                    pair.Value.Calculate(GetAttributeCount(pair.Value.Dependent, false));
                }
            }

            //Set Skill cap to be highest skill costing Spell
            if (Resources.ContainsKey(ResourceType.SKILL) && Spells.Count > 0) {
                Resources[ResourceType.SKILL].True = Mathf.Max(2, Spells.Where(s => s.Costs.ContainsKey(ResourceType.SKILL)).Select(s => s.Costs[ResourceType.SKILL]).OrderByDescending(i => i).FirstOrDefault());
            }
        }
    }
}