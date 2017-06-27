using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Model.Spells;
using UnityEngine;

namespace Scripts.Model.Items {

    public class BasicItem : Item {

        public BasicItem(Sprite icon, int basePrice, string name, string description)
            : base(icon, basePrice, TargetType.NONE, name, description) {

        }

        public override SpellBook GetSpellBook() {
            return new Dummy(this);
        }

        protected sealed override string DescriptionHelper {
            get {
                return string.Empty;
            }
        }

        protected sealed override bool IsMeetOtherRequirements(SpellParams caster, SpellParams target) {
            return true;
        }
    }
}
