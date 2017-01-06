using Scripts.Model.Characters;
using Scripts.Model.Spells;
using UnityEngine;

namespace Scripts.Model.Items {

    /// <summary>
    /// Represents an item that can be used.
    /// </summary>
    public abstract class Item : SpellFactory {
        public const SpellType SPELL_TYPE = SpellType.BOOST;
        public const TargetType TARGET_TYPE = TargetType.SINGLE_ALLY;
        public static readonly Color KEY_ITEM_COLOR = Color.magenta;
        public readonly bool IsKeyItem; // Key items must be looted on a loot page, and cannot be tossed.

        public override string Name {
            get {
                string ret = "";
                if (IsKeyItem) {
                    ret = string.Format("{0}", Util.Color(name, KEY_ITEM_COLOR));
                } else {
                    ret = string.Format("{0}", this.name);
                }
                return ret;
            }
        }

        public Item(string name, string description, bool isKeyItem = false) : base(name, description, SPELL_TYPE, TARGET_TYPE) {
            this.IsKeyItem = isKeyItem;
        }

        public override string GetNameAndCosts(Character caster) {
            string nameAndCount = "";
            if (IsKeyItem) {
                nameAndCount = string.Format("{0}", Util.Color(Name, KEY_ITEM_COLOR));
            } else {
                nameAndCount = string.Format("{0}", this.Name);
            }
            return IsCastable(caster) ? nameAndCount : "<color=red>" + nameAndCount + "</color>";
        }

        protected override bool Castable(Character caster, Character target) {
            return caster.Inventory.Contains(this);
        }
    }
}