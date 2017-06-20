using Scripts.Model.Spells;
using System.Collections;
using System.Collections.Generic;
using Scripts.Model.Stats;
using System;
using UnityEngine;
using Scripts.Model.Buffs;
using Scripts.Game.Defined.Spells;
using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;

namespace Scripts.Model.Items {

    public abstract class EquippableItem : UseableItem, ISaveable<EquipItemSave> {

        public readonly EquipType Type;
        public readonly IDictionary<StatType, int> Stats;

        private readonly SpellBook book;

        private static readonly IDictionary<EquipType, Sprite> DEFAULT_ICONS = new Dictionary<EquipType, Sprite>() {
            { EquipType.ARMOR, Util.GetSprite("shoulder-armor") },
            { EquipType.WEAPON, Util.GetSprite("gladius") },
            {EquipType.TRINKET, Util.GetSprite("gem-necklace") },
            { EquipType.OFFHAND, Util.GetSprite("round-shield") }
        };

        public EquippableItem(Sprite sprite, EquipType type, int basePrice, string name, string description)
            : base(sprite, basePrice, TargetType.SINGLE_ALLY, name, description) {
            this.Type = type;
            this.Stats = new SortedDictionary<StatType, int>();
            this.book = new CastEquipItem(this);
        }

        public EquippableItem(EquipType type, int basePrice, string name, string description)
            : this(GetDefaultSprite(type), type, basePrice, name, description) { }

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

        private static Sprite GetDefaultSprite(EquipType type) {
            return DEFAULT_ICONS[type];
        }

        EquipItemSave ISaveable<EquipItemSave>.GetSaveObject() {
            return new EquipItemSave(new EquipTypeSave(Type), GetType());
        }

        public void InitFromSaveObject(EquipItemSave saveObject) {
            // Nothing
        }
    }
}
