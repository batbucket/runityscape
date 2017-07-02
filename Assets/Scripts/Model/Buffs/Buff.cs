using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
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
        public readonly Characters.Stats Caster;
        public readonly int CasterId;

        public BuffParams(Characters.Stats caster, int casterId) {
            this.Caster = caster;
            this.CasterId = casterId;
        }
    }

    public abstract class Buff : IComparable<Buff>, ISaveable<BuffSave>, IIdNumberable {
        public readonly Sprite Sprite;
        public readonly string Name;
        public readonly string Description;
        public readonly bool IsDispellable;

        private Characters.Stats caster;
        private int turnsRemaining;
        private bool isDefinite;
        private static int idCount;
        private int buffId;
        private int casterId;

        private bool isCasterSet;

        public Buff(Sprite sprite, string name, string description, bool isDispellable) {
            this.Sprite = sprite;
            this.Name = name;
            this.Description = description;
            this.IsDispellable = isDispellable;
            this.buffId = idCount++;
        }

        public Buff(int duration, Sprite sprite, string name, string description, bool isDispellable) : this(sprite, name, description, isDispellable) {
            Util.Assert(duration > 0, "Duration must be positive.");
            this.turnsRemaining = duration;
            this.isDefinite = true;
        }

        public int TurnsRemaining {
            get {
                return turnsRemaining;
            }
        }

        public int Id {
            get {
                return buffId;
            }
        }

        public BuffParams Caster {
            set {
                Util.Assert(!isCasterSet, "Attempted to set caster twice.");
                this.caster = value.Caster;
                this.casterId = value.CasterId;
                isCasterSet = true;
            }
        }

        public Characters.Stats BuffCaster {
            get {
                return caster;
            }
        }

        public int CasterId {
            get {
                return casterId;
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

        public virtual bool IsReact(Spell s) {
            return false;
        }

        public void React(Spell s, Characters.Stats owner) {
            ReactHelper(s, owner);
        }

        public void OnTimeOut(Characters.Stats owner) {
            PerformSpellEffects(OnTimeOutHelper(owner));
        }

        public void OnDispell(Characters.Stats owner) {
            PerformSpellEffects(OnDispellHelper(owner));
        }

        public override bool Equals(object obj) {
            var item = obj as Buff;

            if (item == null) {
                return false;
            }

            return this.GetType().Equals(item.GetType())
                && this.turnsRemaining.Equals(item.turnsRemaining)
                && this.caster.Equals(item.caster);
        }

        public override int GetHashCode() {
            return Name.GetHashCode() ^ Description.GetHashCode();
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

        protected virtual void ReactHelper(Spell s, Characters.Stats owner) {

        }

        protected virtual IList<SpellEffect> OnTimeOutHelper(Characters.Stats owner) {
            return new SpellEffect[0];
        }

        protected virtual IList<SpellEffect> OnDispellHelper(Characters.Stats owner) {
            return OnTimeOutHelper(owner);
        }

        private void PerformSpellEffects(IList<SpellEffect> list) {
            foreach (SpellEffect mySE in list) {
                SpellEffect se = mySE;
                se.CauseEffect();
            }
        }

        public BuffSave GetSaveObject() {
            return new BuffSave(turnsRemaining, caster.GetSaveObject(), casterId, GetType());
        }

        public void InitFromSaveObject(BuffSave saveObject) {
            this.turnsRemaining = saveObject.TurnsRemaining;
            // Setup caster and id in party!
        }
    }
}
