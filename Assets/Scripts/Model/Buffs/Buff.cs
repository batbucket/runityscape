using Scripts.Model.Characters;
using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Model.Spells;
using Scripts.Presenter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Buffs {
    public struct BuffParams {
        public Characters.Stats Caster;
        public int CasterId;
    }

    public abstract class Buff : IComparable<Buff>, ISaveable<PartialBuffSave, FullBuffSave> {
        public readonly Sprite Sprite;
        public readonly string Name;
        public readonly string Description;

        private Characters.Stats caster;
        private int turnsRemaining;
        private bool isDefinite;
        private static int idCount;
        private int buffId;
        private int casterId;

        private bool isCasterSet;

        public Buff(Sprite sprite, string name, string description) {
            this.Sprite = sprite;
            this.Name = name;
            this.Description = description;
            this.buffId = idCount++;
        }

        public Buff(int duration, Sprite sprite, string name, string description) {
            Util.Assert(duration > 0, "Duration must be positive.");
            this.Sprite = sprite;
            this.Name = name;
            this.Description = description;
            this.buffId = idCount++;
            this.turnsRemaining = duration;
            this.isDefinite = true;
        }

        public BuffParams Caster {
            set {
                Util.Assert(!isCasterSet, "Attempted to set caster twice.");
                this.caster = value.Caster;
                this.casterId = value.CasterId;
                isCasterSet = true;
            }
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

        public void OnApply(Characters.Stats owner) {
            PerformSpellEffects(OnApplyHelper(owner));
        }

        public void OnEndOfTurn(Characters.Stats owner) {
            PerformSpellEffects(OnEndOfTurnHelper(owner));
            if (isDefinite) {
                turnsRemaining--;
            }
        }

        public void React(Spell s, Characters.Stats owner) {
            PerformSpellEffects(ReactHelper(s, owner));
        }

        public void OnTimeOut(Characters.Stats owner) {
            PerformSpellEffects(OnTimeOutHelper(owner));
        }

        public void OnDispell(Characters.Stats owner) {
            PerformSpellEffects(OnDispellHelper(owner));
        }

        public override bool Equals(object obj) {
            return this == obj;
        }

        public override int GetHashCode() {
            return this.buffId;
        }

        public int CompareTo(Buff other) {
            return this.turnsRemaining - other.turnsRemaining;
        }

        protected virtual IList<SpellEffect> OnApplyHelper(Characters.Stats owner) {
            return new SpellEffect[0];
        }

        protected virtual IList<SpellEffect> OnEndOfTurnHelper(Characters.Stats owner) {
            return new SpellEffect[0];
        }

        protected virtual IList<SpellEffect> ReactHelper(Spell s, Characters.Stats owner) {
            return new SpellEffect[0];
        }

        protected virtual IList<SpellEffect> OnTimeOutHelper(Characters.Stats owner) {
            return new SpellEffect[0];
        }

        protected virtual IList<SpellEffect> OnDispellHelper(Characters.Stats owner) {
            return new SpellEffect[0];
        }

        private void PerformSpellEffects(IList<SpellEffect> list) {
            foreach (SpellEffect mySE in list) {
                SpellEffect se = mySE;
                se.CauseEffect();
            }
        }

        public PartialBuffSave GetSaveObject() {
            return new PartialBuffSave(turnsRemaining, caster.GetSaveObject(), casterId, GetType());
        }

        public void InitFromSaveObject(FullBuffSave saveObject) {
            this.turnsRemaining = saveObject.TurnsRemaining;
            // Setup caster and id in party!
        }
    }
}
