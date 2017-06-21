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

        public AddBuff(BuffParams caster, Buffs target, Buff buff) : base(SpellEffectType.ADD_BUFF, 1) {
            this.target = target;
            this.buff = buff;
            this.buff.Caster = caster;
        }

        public override void CauseEffect() {
            target.AddBuff(buff);
        }
    }

    public struct EquipParams {
        public readonly Inventory Caster;
        public readonly Equipment Target;
        public readonly EquippableItem Item;

        public EquipParams(Inventory caster, Equipment target, EquippableItem item) {
            this.Caster = caster;
            this.Target = target;
            this.Item = item;
        }
    }

    public class EquipItemEffect : SpellEffect {
        private EquipParams ep;

        public EquipItemEffect(EquipParams ep) : base(SpellEffectType.EQUIP_ITEM, 1) {
            this.ep = ep;
        }

        public override void CauseEffect() {
            ep.Target.AddEquip(ep.Caster, ep.Item);
        }
    }

    public class UnequipItemEffect : SpellEffect {
        private EquipParams ep;

        public UnequipItemEffect(EquipParams ep) : base(SpellEffectType.EQUIP_ITEM, 1) {
            this.ep = ep;
        }

        public override void CauseEffect() {
            ep.Target.RemoveEquip(ep.Caster, ep.Item.Type);
        }
    }

    public class ConsumeItemEffect : SpellEffect {
        private Inventory caster;
        private Item item;

        public ConsumeItemEffect(Item item, Inventory caster) : base(SpellEffectType.CONSUME_ITEM, 1) {
            this.caster = caster;
            this.item = item;
        }

        public override void CauseEffect() {
            caster.Remove(item, 1);
        }
    }
}