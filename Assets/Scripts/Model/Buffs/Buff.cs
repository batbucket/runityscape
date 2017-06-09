using Scripts.Model.Characters;
using Scripts.Model.Spells;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Buffs {

    public abstract class Buff : IComparable<Buff> {
        public readonly Sprite Sprite;
        public readonly string Name;
        public readonly string Description;

        public readonly Characters.Stats Caster;
        public readonly Characters.Stats Target;

        public int TurnsRemaining;
        public bool IsDefinite;

        private static int idCount;
        private int id;

        public Buff(Characters.Stats caster, Characters.Stats target, Sprite sprite, string name, string description) {
            this.Caster = caster;
            this.Target = target;
            this.Sprite = sprite;
            this.Name = name;
            this.Description = description;
            this.id = id;
        }

        public virtual IList<SpellEffect> OnApply() {
            return new SpellEffect[0];
        }

        public virtual IList<SpellEffect> OnEndOfTurn() {
            return new SpellEffect[0];
        }

        public virtual IList<SpellEffect> React(Spell s) {
            return new SpellEffect[0];
        }

        public virtual IList<SpellEffect> OnTimeOut() {
            return new SpellEffect[0];
        }

        public virtual IList<SpellEffect> OnDispell() {
            return new SpellEffect[0];
        }

        public override bool Equals(object obj) {
            return this == obj;
        }

        public override int GetHashCode() {
            return this.id;
        }

        public int CompareTo(Buff other) {
            return this.TurnsRemaining - other.TurnsRemaining;
        }
    }
}
