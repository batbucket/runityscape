using Scripts.Model.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scripts.Model.Interfaces;

namespace Scripts.Model.Items {
    public abstract class UseableItem : Item, ISpellable {
        public UseableItem(Sprite sprite, int basePrice, int count, int maxCount, TargetType target, string name, string description)
            : base(sprite, basePrice, count, maxCount, target, name, description) {
            flags.Add(Flag.USABLE);
        }

        public abstract SpellBook GetSpellBook();
    }

}

