using Scripts.Model.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scripts.Model.Interfaces;
using Scripts.Model.Characters;

namespace Scripts.Model.Items {
    public abstract class UseableItem : Item, ISpellable {
        public UseableItem(Sprite sprite, int basePrice, TargetType target, string name, string description)
            : base(sprite, basePrice, target, name, description) {
            flags.Add(Flag.USABLE);
            flags.Add(Flag.OCCUPIES_SPACE);
        }

        public abstract SpellBook GetSpellBook();
    }

}

