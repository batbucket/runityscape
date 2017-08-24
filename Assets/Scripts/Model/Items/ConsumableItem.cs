using Scripts.Model.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scripts.Game.Defined.Spells;
using Scripts.Game.Defined.Serialized.Spells;

namespace Scripts.Model.Items {

    public abstract class ConsumableItem : UseableItem {
        private readonly SpellBook book;

        private static readonly Sprite DEFAULT_SPRITE = Util.GetSprite("shiny-apple");

        public ConsumableItem(Sprite sprite, int basePrice, TargetType target, string name, string description)
            : base(sprite, basePrice, target, name, description) {
            this.book = new UseItem(this);
        }

        public ConsumableItem(int basePrice, TargetType target, string name, string description)
            : this(DEFAULT_SPRITE, basePrice, target, name, description) { }

        public sealed override SpellBook GetSpellBook() {
            return book;
        }

        public abstract IList<SpellEffect> GetEffects(SpellParams caster, SpellParams target);

        protected override bool IsMeetOtherRequirements(SpellParams caster, SpellParams target) {
            return true;
        }

        protected sealed override string DescriptionHelper {
            get {
                return string.Format("Consumable\n{0}", Flavor);
            }
        }

    }
}
