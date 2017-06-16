using Scripts.Model.Spells;
using System.Collections;
using System.Collections.Generic;
using Scripts.Model.Stats;
using System;
using UnityEngine;
using Scripts.Model.Buffs;
using Scripts.Game.Defined.Spells;

namespace Scripts.Model.Items {

    public abstract class EquippableItem : UseableItem {
        private const int STACK_LIMIT = 5;

        public readonly EquipType Type;
        public readonly IDictionary<StatType, int> Stats;

        private readonly SpellBook book;

        public EquippableItem(Sprite sprite, EquipType type, int basePrice, int count, string name, string description)
            : base(sprite, basePrice, count, STACK_LIMIT, TargetType.SINGLE_ALLY, name, description) {
            this.Type = type;
            this.Stats = new SortedDictionary<StatType, int>();
            this.book = new CastEquipItem(this);
        }

        protected sealed override string DescriptionHelper {
            get {
                string[] arr = new string[Stats.Count];

                int index = 0;
                foreach (KeyValuePair<StatType, int> pair in Stats) {
                    arr[index++] = string.Format("{0} {1}", StatUtil.ShowSigns(pair.Value), pair.Key.Name);
                }
                return string.Format("{0}\n{1}", string.Join("\n", arr), Util.ColorString(Flavor, Color.yellow));
            }
        }

        public virtual Buff CreateBuff(SpellParams target) {
            return null;
        }

        public sealed override SpellBook GetSpellBook() {
            return book;
        }

        protected override bool IsMeetOtherRequirements(SpellParams caster, SpellParams target) {
            return caster.Stats.State == Characters.State.ALIVE && target.Stats.State == Characters.State.ALIVE;
        }
    }
}
