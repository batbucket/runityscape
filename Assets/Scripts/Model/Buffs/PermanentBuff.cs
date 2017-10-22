using System.Collections.Generic;
using Scripts.Game.Defined.Serialized.Buffs;
using Scripts.Model.Characters;
using Scripts.Model.Spells;
using UnityEngine;

namespace Scripts.Model.Buffs {

    /// <summary>
    /// Equipment buffs have infinite duration, are not dispellable, and can noly apply OnEndOfTurn and Reactionary effects.
    /// </summary>
    /// <seealso cref="Scripts.Model.Buffs.Buff" />
    public abstract class PermanentBuff : Buff {

        public PermanentBuff(Sprite sprite, string name, string description) : base(sprite, name, description, false) {
        }

        public override bool Equals(object obj) {
            PermanentBuff item = obj as PermanentBuff;
            if (item == null) {
                return false;
            }
            return (this.GetType().Equals(item.GetType()));
        }

        public override int GetHashCode() {
            return Name.GetHashCode() ^ Description.GetHashCode();
        }

        protected override sealed IList<SpellEffect> OnApplyHelper(Characters.Stats ownerOfThisBuff) {
            return new SpellEffect[0];
        }

        protected override sealed IList<SpellEffect> OnDispellHelper(Characters.Stats ownerOfThisBuff) {
            return new SpellEffect[0];
        }

        protected override sealed IList<SpellEffect> OnTimeOutHelper(Characters.Stats ownerOfThisBuff) {
            return new SpellEffect[0];
        }
    }
}