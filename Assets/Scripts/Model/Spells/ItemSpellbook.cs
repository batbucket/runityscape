using Scripts.Model.Items;
using System;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Model.Characters;

namespace Scripts.Model.Spells {
    public abstract class ItemSpellBook : SpellBook {
        protected Item item;

        public ItemSpellBook(Item item) : base(item.Name, item.Icon, item.Target, SpellType.ITEM, 0, 0) {
            this.item = item;
            flags.Remove(Flag.CASTER_REQUIRES_SPELL);
        }

        public override string GetDetailedName(SpellParams caster) {
            int count = caster.Inventory.GetCount(item);

            Color color = Color.white;
            if (!IsMeetPreTargetRequirements(caster.Stats)) {
                color = Color.grey;
            }

            return string.Format("{0}{1}",
                Util.ColorString(Name, color),
                count > 1 ? string.Format("({0})", count) : string.Empty
                );
        }

        protected override bool IsMeetOtherCastRequirements(SpellParams caster, SpellParams target) {
            return item.IsUsable(caster, target) && IsMeetOtherCastRequirements2(caster, target);
        }

        protected virtual bool IsMeetOtherCastRequirements2(SpellParams caster, SpellParams target) {
            return true;
        }
    }
}
