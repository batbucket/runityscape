using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Model.Stats;
using Scripts.View.Portraits;
using Scripts.Model.Items;
using Scripts.Model.Spells;
using Scripts.Game.Defined.SFXs;
using Scripts.Model.Characters;

namespace Scripts.Game.Defined.Spells {

    public class Attack : BasicSpellbook {
        public const float PERCENT = 0.01f;
        public const float CRITICAL_MULTIPLIER = 2f;
        public const int SKILL_ON_HIT = 1;
        public const int SKILL_ON_CRIT = 2;

        public Attack() : base("Attack", Util.GetSprite("gladius"), TargetType.SINGLE_ENEMY, SpellType.OFFENSE) { }

        public override string CreateDescriptionHelper(SpellParams caster) {
            return string.Format("A basic attack with the weapons or fists.");
        }

        protected override IList<IEnumerator> GetHitSFX(PortraitView caster, PortraitView target) {
            return new IEnumerator[] { SFX.Melee(caster.Image.gameObject, target.Image.gameObject, 0.5f, "Attack_0") };
        }

        protected override bool IsHit(SpellParams caster, SpellParams target) {
            // 1 + (c.Agi - t.Agi)%
            return Util.IsChance(1 + StatUtil.GetDifference(StatType.AGILITY, caster.Stats, target.Stats) * PERCENT);
        }

        protected override bool IsCritical(SpellParams caster, SpellParams target) {
            // (c.Int - t.Int)% chance to critical
            return Util.IsChance(StatUtil.GetDifference(StatType.INTELLECT, caster.Stats, target.Stats) * PERCENT);
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] {
                    new AddToModStat(target.Stats, StatType.HEALTH, -Util.Random(caster.Stats.GetStatCount(Value.MOD, StatType.STRENGTH), .25f)),
                    new AddToModStat(caster.Stats, StatType.SKILL, 1)
                };
        }

        protected override IList<SpellEffect> GetCriticalEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] {
                    new AddToModStat(target.Stats, StatType.HEALTH, -Util.Random(caster.Stats.GetStatCount(Value.MOD, StatType.STRENGTH) * CRITICAL_MULTIPLIER, .25f)),
                    new AddToModStat(caster.Stats, StatType.SKILL, 2)
                };
        }
    }

    public class Wait : BasicSpellbook {
        public Wait() : base("Wait", Util.GetSprite("hourglass"), TargetType.SELF, SpellType.MERCY) { }

        public override string CreateDescriptionHelper(SpellParams caster) {
            return "Literally do nothing.";
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[0];
        }
    }

    public class InflictPoison : BasicSpellbook {
        public InflictPoison() : base("Poison", Util.GetSprite("fox-head"), TargetType.SINGLE_ENEMY, SpellType.OFFENSE) {
            Costs.Add(StatType.MANA, 1);
        }

        public override string CreateDescriptionHelper(SpellParams caster) {
            return string.Format("Enemy is afflicted with Poison.");
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] { new AddBuff(target.Buffs, new Poison(caster, target)) };
        }
    }


    public class CastEquipItem : ItemSpellbook {
        private EquippableItem equip;

        public CastEquipItem(EquippableItem equip) : base(equip) {
            this.equip = equip;
        }

        public override string CreateDescriptionHelper(SpellParams caster) {
            return string.Format("{0}\n\nEquip [{1}] in a target's [{2}] equipment slot.", equip.Description, equip.Name, equip.Type);
        }

        protected override bool IsMeetOtherCastRequirements2(SpellParams caster, SpellParams target) {
            return !target.Equipment.Contains(equip.Type) || caster.Inventory.IsAddable(target.Equipment.PeekItem(equip.Type));
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] {
                    new EquipItemEffect(new EquipParams(caster.Inventory, target.Equipment, equip))
                };
        }
    }

    public class CastUnequipItem : ItemSpellbook {
        private Inventory caster;
        private Equipment targetEq;
        private new EquippableItem item;

        public CastUnequipItem(Inventory caster, Equipment targetEq, EquippableItem item) : base(item) {
            this.caster = caster;
            this.targetEq = targetEq;
            this.item = item;
        }

        public override string CreateDescriptionHelper(SpellParams caster) {
            return string.Format("{0}\n\nUnequip [{1}] from the [{2}] equipment slot.", item.Description, item.Name, item.Type);
        }

        protected override bool IsMeetOtherCastRequirements2(SpellParams caster, SpellParams target) {
            return target.Equipment.Contains(item.Type) || caster.Inventory.IsAddable(item);
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] {
                    new EquipItemEffect(new EquipParams(caster.Inventory, target.Equipment, item))
                };
        }
    }

    public class UseItem : ItemSpellbook {
        private readonly ConsumableItem consume;

        public UseItem(ConsumableItem consume) : base(consume) {
            this.consume = consume;
        }

        public override string CreateDescriptionHelper(SpellParams caster) {
            return string.Format("{0}\n\nUse [{1}] on a target.", item.Description, item.Name);
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            IList<SpellEffect> itemEffects = consume.GetEffects(caster, target);
            SpellEffect[] allEffects = new SpellEffect[itemEffects.Count + 1];
            allEffects[0] = new ConsumeItemEffect(consume, caster.Inventory);
            for (int i = 0; i < itemEffects.Count; i++) {
                allEffects[i + 1] = itemEffects[i];
            }
            return allEffects;
        }
    }

}
