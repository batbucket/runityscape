using System;
using System.Collections.Generic;
using Scripts.Model.Characters;
using Scripts.Model.Stats;
using UnityEngine;
using System.Text;
using Scripts.Model.Buffs;
using Scripts.Model.Items;
using Scripts.Model.Spells;

namespace Scripts.Game.Defined.Spells {
    public class AddToModStat : SpellEffect {
        private readonly StatType affected;
        private readonly Stats target;

        public AddToModStat(Stats target, StatType affected, int value) : base(SpellEffectType.ADD_TO_MOD_STAT, value) {
            this.affected = affected;
            this.target = target;
        }

        public override void CauseEffect() {
            target.AddToStat(affected, Model.Characters.Value.MOD, Value);
        }
    }

    public class AddBuff : SpellEffect {
        private readonly Buff buff;
        private readonly Buffs target;

        public AddBuff(Buffs target, Buff buff) : base(SpellEffectType.ADD_BUFF, 1) {
            this.target = target;
            this.buff = buff;
        }

        public override void CauseEffect() {
            target.AddBuff(buff);
        }
    }

    public class EquipItemEffect : SpellEffect {
        private Equipment target;
        private EquippableItem item;

        public EquipItemEffect(Equipment target, EquippableItem item) : base(SpellEffectType.EQUIP_ITEM, 1) {
            this.target = target;
            this.item = item;
        }

        public override void CauseEffect() {
            target.AddEquip(item);
        }
    }

    public class UnequipItemEffect : SpellEffect {
        private Equipment target;
        private EquipType itemType;

        public UnequipItemEffect(Equipment target, EquipType itemType) : base(SpellEffectType.EQUIP_ITEM, 1) {
            this.target = target;
            this.itemType = itemType;
        }

        public override void CauseEffect() {
            target.RemoveEquip(itemType);
        }
    }
}