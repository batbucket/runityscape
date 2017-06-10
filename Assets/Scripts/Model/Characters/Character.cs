using Scripts.Model.Interfaces;
using Scripts.Model.Perks;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using Scripts.Model.TextBoxes;
using Scripts.Presenter;
using Scripts.View.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Scripts.Model.Characters {

    /// <summary>
    /// Characters are special entities with
    /// Resources and Attributes, as well as numerous SpellFactories.
    ///
    /// They can participate in battles.
    /// </summary>
    public class Character : IComparable<Character> {

        public Stats Stats;
        public Buffs Buffs;
        public Look Look;
        public Spells Spells;
        public CharacterPresenter Presenter;
        public Brain Brain;

        private static int idCounter;
        private int id;

        public Character(Stats stats, Look look, Brain brain, Spells spells) {
            this.Stats = stats;
            this.Buffs = new Buffs();
            this.Brain = brain;
            this.Look = look;
            this.Spells = spells;
            Brain.Owner = this;
            Brain.Spells = this.Spells;
            Stats.Update(this);
            Stats.InitializeResources();
            this.id = idCounter++;
        }

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