using Scripts.Model.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scripts.Game.Defined.Spells;

namespace Scripts.Model.Items {

    public abstract class ConsumableItem : UseableItem {
        private const int STACK_LIMIT = 5;
        private readonly SpellBook book;

        public ConsumableItem(Sprite sprite, int basePrice, int count, TargetType target, string name, string description)
            : base(sprite, basePrice, count, STACK_LIMIT, target, name, description) {
            this.book = new UseItem(this);
        }

        public sealed override SpellBook GetSpellBook() {
            return book;
        }

        public abstract IList<SpellEffect> GetEffects(SpellParams caster, SpellParams target);

        protected override bool IsMeetOtherRequirements(SpellParams caster, SpellParams target) {
            return true;
        }

        protected override string DescriptionHelper {
            get {
                return string.Empty;
            }
        }

    }
}
