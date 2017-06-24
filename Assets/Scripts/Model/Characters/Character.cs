using Scripts.Model.Interfaces;
using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using Scripts.Model.TextBoxes;
using Scripts.Presenter;
using System;

namespace Scripts.Model.Characters {

    /// <summary>
    /// Characters are special entities with
    /// Resources and Attributes, as well as numerous SpellFactories.
    ///
    /// They can participate in battles.
    /// </summary>
    public class Character : IComparable<Character>, ISaveable<CharacterSave>, IIdNumberable {
        public const int UNKNOWN_ID = -1;

        public readonly Stats Stats;
        public readonly Buffs Buffs;
        public readonly Look Look;
        public readonly SpellBooks Spells;
        public Brain Brain;
        public Inventory Inventory;
        public readonly Equipment Equipment;

        public CharacterPresenter Presenter;

        private static int idCounter;
        private int id;

        public Character(Stats stats, Look look, Brain brain, SpellBooks spells, Inventory inventory, Equipment equipment) {
            this.Stats = stats;
            this.Buffs = new Buffs();
            this.Brain = brain;
            this.Look = look;
            this.Spells = spells;
            this.Inventory = inventory;
            this.Equipment = equipment;

            Brain.Owner = this;
            Brain.Spells = this.Spells;
            Stats.Update(this);
            Equipment.AddBuff = b => Buffs.AddBuff(b);
            Equipment.RemoveBuff = b => Buffs.RemoveBuff(RemovalType.DISPEL, b);
            Stats.InitializeResources();
            Stats.GetEquipmentBonus = f => Equipment.GetBonus(f);
            Buffs.Stats = Stats;
            this.id = idCounter++;
        }

        public Character(Stats stats, Look look, Brain brain, SpellBooks spells) : this(stats, look, brain, spells, new Inventory(), new Equipment()) { }

        /// <summary>
        /// Used in serialization for parties
        /// </summary>
        /// <param name="inventory">Reference to the shared inventory</param>
        public Character(Inventory inventory) : this(new Stats(), new Look(), new Game.Defined.Serialized.Characters.Player(), new SpellBooks(), inventory, new Equipment()) { }

        public int Id {
            get {
                return id;
            }
        }

        public TextBox Emote(string s) {
            return new TextBox(string.Format(s, this.Look.DisplayName));
        }

        public void Update() {
            Stats.Update(this);
        }

        public override bool Equals(object obj) {
            var item = obj as Character;

            if (item == null) {
                return false;
            }

            return this.Stats.Equals(item.Stats)
                && this.Buffs.Equals(item.Buffs)
                && this.Look.Equals(item.Look)
                && this.Spells.Equals(item.Spells)
                && this.Brain.Equals(item.Brain)
                && this.Inventory.Equals(item.Inventory)
                && this.Equipment.Equals(item.Equipment);
        }

        public override int GetHashCode() {
            return 0;
        }

        public virtual void OnBattleStart() {
        }

        public virtual void OnKill() {

        }

        public int CompareTo(Character other) {
            int diff = other.Stats.GetStatCount(Value.MOD, StatType.AGILITY) - this.Stats.GetStatCount(Value.MOD, StatType.AGILITY);
            if (diff == 0) {
                return (Util.IsChance(.5) ? -1 : 1);
            } else {
                return diff;
            }
        }

        public CharacterSave GetSaveObject() {
            return new CharacterSave(
                this.id,
                Stats.GetSaveObject(),
                Buffs.GetSaveObject(),
                Look.GetSaveObject(),
                Spells.GetSaveObject(),
                Brain.GetSaveObject(),
                Equipment.GetSaveObject()
                );
        }

        public void InitFromSaveObject(CharacterSave saveObject) {
            // No special classes - set fields
            this.Stats.InitFromSaveObject(saveObject.Stats);
            this.Look.InitFromSaveObject(saveObject.Look);
            this.Spells.InitFromSaveObject(saveObject.Spells);
            this.Equipment.InitFromSaveObject(saveObject.Equipment);

            // Must replace class
            Brain brain = saveObject.Brain.CreateObjectFromID();
            this.Brain = brain;

            // Buffs and inventory must be setup in Party!
        }
    }
}