using Scripts.Model.Interfaces;
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
    public class Character : IComparable<Character> {

        public readonly Stats Stats; // g
        public readonly Buffs Buffs;
        public readonly Look Look; // g
        public readonly SpellBooks Spells; // g
        public readonly Brain Brain; // g
        public readonly Inventory Inventory; // g
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
            Equipment.RemoveBuff = b => Buffs.RemoveBuff(RemovalType.TIMED_OUT, b);
            Stats.InitializeResources();
            Stats.GetEquipmentBonus = f => Equipment.GetBonus(f);
            equipment.Owner = new SpellParams(this);
            this.id = idCounter++;
        }

        public Character(Stats stats, Look look, Brain brain, SpellBooks spells) : this(stats, look, brain, spells, new Inventory(), new Equipment()) { }

        public TextBox Emote(string s) {
            return new TextBox(string.Format(s, this.Look.DisplayName));
        }

        public void Update() {
            Stats.Update(this);
        }

        public override bool Equals(object obj) {
            return this == obj;
        }

        public override int GetHashCode() {
            return id;
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
    }
}