using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Model.Stats;
using Scripts.View.Portraits;
using Scripts.Model.Items;
using Scripts.Model.Spells;
using Scripts.Game.Defined.SFXs;
using Scripts.Model.Characters;
using Scripts.Game.Defined.Spells;
using Scripts.Model.Buffs;

namespace Scripts.Game.Defined.Serialized.Spells {

    public class Attack : BasicSpellbook {
        public const float PERCENT = 0.01f;
        public const float CRITICAL_MULTIPLIER = 2f;
        public const float BASE_ACCURACY = .8f;
        public const float BASE_CRITICAL_RATE = .2f;
        public const int SKILL_ON_HIT = 1;
        public const int SKILL_ON_CRIT = 2;

        public Attack() : base("Attack", Util.GetSprite("gladius"), TargetType.SINGLE_ENEMY, SpellType.OFFENSE) { }

        public override string CreateDescriptionHelper(SpellParams caster) {
            return string.Format("A basic attack with the weapons or fists.");
        }

        protected override IList<IEnumerator> GetHitSFX(PortraitView caster, PortraitView target) {
            return new IEnumerator[] { SFX.Melee(caster.Image.gameObject, target.Image.gameObject, 0.5f, "Slash_0") };
        }

        protected override bool IsHit(SpellParams caster, SpellParams target) {
            return Util.IsChance(BASE_ACCURACY + StatUtil.GetDifference(StatType.AGILITY, caster.Stats, target.Stats) * PERCENT);
        }

        protected override bool IsCritical(SpellParams caster, SpellParams target) {
            return Util.IsChance(BASE_CRITICAL_RATE + StatUtil.GetDifference(StatType.AGILITY, caster.Stats, target.Stats) * PERCENT);
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] {
                    new AddToModStat(target.Stats, StatType.HEALTH, -Util.Random(caster.Stats.GetStatCount(Stats.Get.MOD, StatType.STRENGTH), .25f)),
                    new AddToModStat(caster.Stats, StatType.SKILL, 1)
                };
        }

        protected override IList<SpellEffect> GetCriticalEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] {
                    new AddToModStat(target.Stats, StatType.HEALTH, -Util.Random(caster.Stats.GetStatCount(Stats.Get.MOD, StatType.STRENGTH) * CRITICAL_MULTIPLIER, .25f)),
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

    public class Check : BasicSpellbook {
        public Check() : base("Check", Util.GetSprite("magnifying-glass"), TargetType.SINGLE_ENEMY, SpellType.OFFENSE) { }

        public override string CreateDescriptionHelper(SpellParams caster) {
            Checked dummy = new Checked();
            return string.Format("Closely analyze an enemy, temporarily increasing their resource visibility.", dummy.Name, dummy.Description);
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] { new AddBuff(new BuffParams(caster.Stats, caster.CharacterId), target.Buffs, new Checked()) };
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
            return new SpellEffect[] { new AddBuff(new BuffParams(caster.Stats, caster.CharacterId), target.Buffs, new Poison()) };
        }
    }
}
