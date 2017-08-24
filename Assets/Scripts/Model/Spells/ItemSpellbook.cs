using Scripts.Model.Items;
using System;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Model.Characters;

namespace Scripts.Model.Spells {
    public abstract class ItemSpellBook : SpellBook {
        protected Item item;

        public ItemSpellBook(Item item, string verb) : this(item, item.Name, verb) { }

        public ItemSpellBook(Item item, string name, string verb) : base(name, item.Icon, item.Target, SpellType.ITEM, 0, 0, verb) {
            this.item = item;
            flags.Remove(Flag.CASTER_REQUIRES_SPELL);
        }

        public override string GetDetailedName(SpellParams caster) {
            int count = caster.Inventory.GetCount(item);

            Color color = Color.white;
            if (!IsMeetPreTargetRequirements(caster.Stats)) {
                color = Color.grey;
            }

            return caster.Inventory.CountedItemName(item);
        }

        protected override string CreateDescriptionHelper() {
            return item.Description;
        }

        protected override bool IsMeetOtherCastRequirements(SpellParams caster, SpellParams target) {
            return item.IsUsable(caster, target) && IsMeetOtherCastRequirements2(caster, target);
        }

        protected virtual bool IsMeetOtherCastRequirements2(SpellParams caster, SpellParams target) {
            return true;
        }
    }
}
