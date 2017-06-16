using Scripts.Model.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Spells {
    public abstract class ItemSpellbook : SpellBook {
        protected Item item;

        public ItemSpellbook(Item item) : base(item.Name, item.Icon, item.Target, SpellType.ITEM, 0, 0) {
            this.item = item;
        }

        protected override bool IsMeetOtherCastRequirements(SpellParams caster, SpellParams target) {
            return item.IsUsable(caster, target) && IsMeetOtherCastRequirements2(caster, target);
        }

        protected virtual bool IsMeetOtherCastRequirements2(SpellParams caster, SpellParams target) {
            return true;
        }
    }
}
