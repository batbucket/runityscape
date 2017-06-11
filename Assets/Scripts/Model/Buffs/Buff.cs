using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Presenter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Buffs {

    public abstract class Buff : IComparable<Buff> {
        public readonly Sprite Sprite;
        public readonly string Name;
        public readonly string Description;

        public readonly SpellParams Caster;
        public readonly SpellParams Target;

        private GameObject hitsplatsHolder;
        private int turnsRemaining;
        private bool isDefinite;
        private static int idCount;
        private int id;

        public Buff(SpellParams caster, SpellParams target, Sprite sprite, string name, string description) {
            this.Caster = caster;
            this.Target = target;
            this.Sprite = sprite;
            this.Name = name;
            this.Description = description;
            this.id = idCount++;
        }

        public Buff(int duration, SpellParams caster, SpellParams target, Sprite sprite, string name, string description) {
            Util.Assert(duration > 0, "Duration must be positive.");
            this.Caster = caster;
            this.Target = target;
            this.Sprite = sprite;
            this.Name = name;
            this.Description = description;
            this.id = idCount++;
            this.turnsRemaining = duration;
            this.isDefinite = true;
        }

        public bool IsTimedOut {
            get {
                return isDefinite && turnsRemaining <= 0;
            }
        }

        public string DurationText {
            get {
                string s = string.Empty;
                if (isDefinite) {
                    s = turnsRemaining.ToString();
                } else {
                    s = "inf";
                }
                return s;
            }
        }

        public void OnApply() {
            PerformSpellEffects(OnApplyHelper());
        }

        public void OnEndOfTurn() {
            PerformSpellEffects(OnEndOfTurnHelper());
            if (isDefinite) {
                turnsRemaining--;
            }
        }

        public void React(Spell s) {
            PerformSpellEffects(ReactHelper(s));
        }

        public void OnTimeOut() {
            PerformSpellEffects(OnTimeOutHelper());
        }

        public void OnDispell() {
            PerformSpellEffects(OnDispellHelper());
        }

        public override bool Equals(object obj) {
            return this == obj;
        }

        public override int GetHashCode() {
            return this.id;
        }

        public int CompareTo(Buff other) {
            return this.turnsRemaining - other.turnsRemaining;
        }

        protected virtual IList<SpellEffect> OnApplyHelper() {
            return new SpellEffect[0];
        }

        protected virtual IList<SpellEffect> OnEndOfTurnHelper() {
            return new SpellEffect[0];
        }

        protected virtual IList<SpellEffect> ReactHelper(Spell s) {
            return new SpellEffect[0];
        }

        protected virtual IList<SpellEffect> OnTimeOutHelper() {
            return new SpellEffect[0];
        }

        protected virtual IList<SpellEffect> OnDispellHelper() {
            return new SpellEffect[0];
        }

        private void PerformSpellEffects(IList<SpellEffect> list) {
            foreach (SpellEffect mySE in list) {
                SpellEffect se = mySE;
                se.CauseEffect();
            }
        }
    }
}
