using System;
using System.Collections.Generic;
using Scripts.Model.Characters;
using Scripts.Model.Stats;
using UnityEngine;
using System.Text;
using Scripts.Model.Buffs;
using Scripts.Model.Items;
using Scripts.Model.Spells;
using Scripts.Model.Pages;

namespace Scripts.Game.Defined.Spells {
    public class AddToModStat : SpellEffect {
        private readonly StatType affected;
        private readonly Stats target;

        public AddToModStat(Stats target, StatType affected, int value) : base(value) {
            this.affected = affected;
            this.target = target;
        }

        public override void CauseEffect() {
            target.AddToStat(affected, Stats.Set.MOD, Value);
        }
    }

    public class AddBuff : SpellEffect {
        private readonly Buff buff;
        private readonly Buffs target;

        public AddBuff(BuffParams caster, Buffs target, Buff buff) : base(1) {
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
        private BuffParams bp;

        public EquipItemEffect(EquipParams ep, BuffParams bp) : base(1) {
            this.ep = ep;
            this.bp = bp;
        }

        public override void CauseEffect() {
            ep.Target.AddEquip(ep.Caster, bp, ep.Item);
        }
    }

    public class UnequipItemEffect : SpellEffect {
        private EquipParams ep;

        public UnequipItemEffect(EquipParams ep) : base(1) {
            this.ep = ep;
        }

        public override void CauseEffect() {
            ep.Target.RemoveEquip(ep.Caster, ep.Item.Type);
        }
    }

    public class ConsumeItemEffect : SpellEffect {
        private Inventory caster;
        private Item item;

        public ConsumeItemEffect(Item item, Inventory caster) : base(1) {
            this.caster = caster;
            this.item = item;
        }

        public override void CauseEffect() {
            caster.Remove(item, Value);
        }
    }

    public class AddToResourceVisibility : SpellEffect {
        private Stats target;

        public AddToResourceVisibility(Stats target, int value) : base(value) {
            this.target = target;
        }

        public override void CauseEffect() {
            if (Value > 0) {
                target.IncreaseResourceVisibility();
            } else if (Value < 0) {
                target.DecreaseResourceVisibility();
            }
        }
    }

    public class PostText : SpellEffect {
        private string message;

        public PostText(string message) : base(1) {
            this.message = message;
        }

        public override void CauseEffect() {
            Page.TypeText(new Model.TextBoxes.TextBox(message));
        }
    }
}