using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Model.Spells;
using UnityEngine;
using Scripts.Model.Characters;

namespace Scripts.Model.Items {

    /// <summary>
    /// Items that cannot be used. Key items and money.
    /// </summary>
    public class BasicItem : Item {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="icon">Sprite for the item</param>
        /// <param name="basePrice">Base price to be modified a shopkeeper to determine pricing</param>
        /// <param name="name">Item's name</param>
        /// <param name="description">Item's description</param>
        public BasicItem(Sprite icon, int basePrice, string name, string description)
            : base(icon, basePrice, TargetType.NONE, name, description) {
        }

        /// <summary>
        /// Get the associated spellbook
        /// </summary>
        /// <returns>A dummy spellbook since you can't use this item.</returns>
        public override SpellBook GetSpellBook() {
            return new Dummy(this);
        }

        /// <summary>
        /// Only shows the flavor text, since there's nothing else to show.
        /// </summary>
        protected sealed override string DescriptionHelper {
            get {
                return string.Format("{0}", Flavor);
            }
        }

        /// <summary>
        /// This is to avoid the text becoming red.
        /// </summary>
        /// <param name="caster">Item User</param>
        /// <param name="target">Item Target</param>
        /// <returns></returns>
        protected sealed override bool IsMeetOtherRequirements(Character caster, Character target) {
            return true;
        }
    }
}