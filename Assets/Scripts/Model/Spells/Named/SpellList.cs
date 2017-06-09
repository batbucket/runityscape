using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Model.Characters;
using UnityEngine;
using Scripts.Model.Stats;
using Scripts.View.Portraits;

namespace Scripts.Model.Spells {
    public static class SpellList {
        public class Attack : SpellBook {
            public const float PERCENT = 0.01f;
            public const float CRITICAL_MULTIPLIER = 2f;
            public const int SKILL_ON_HIT = 1;
            public const int SKILL_ON_CRIT = 2;

            public Attack() : base("Attack", "fox-head", TargetType.SINGLE_ENEMY, SpellType.OFFENSE, 0, 0) { }

            public override string CreateDescriptionHelper(Characters.Stats caster) {
                return string.Format("A basic attack with the weapons or fists.");
            }

            protected override IList<IEnumerator> GetHitSFX(PortraitView caster, PortraitView target) {
                return new IEnumerator[] { SFXList.Melee(caster.Image.gameObject, target.Image.gameObject, 0.5f, "Attack_0") };
            }

            protected override bool IsHit(Characters.Stats caster, Characters.Stats target) {
                // 1 + (c.Agi - t.Agi)%
                return Util.IsChance(1 + StatUtil.GetDifference(StatType.AGILITY, caster, target) * PERCENT);
            }

            protected override bool IsCritical(Characters.Stats caster, Characters.Stats target) {
                // (c.Int - t.Int)% chance to critical
                return Util.IsChance(StatUtil.GetDifference(StatType.INTELLECT, caster, target) * PERCENT);
            }

            protected override IList<SpellEffect> GetHitEffects(Characters.Stats caster, Characters.Stats target) {
                return new SpellEffect[] {
                    new SpellEffectList.AddToModStat(target, StatType.HEALTH, -Util.Random(caster.GetStatCount(Value.MOD, StatType.STRENGTH), .25f)),
                    new SpellEffectList.AddToModStat(caster, StatType.SKILL, 1)
                };
            }

            protected override IList<SpellEffect> GetCriticalEffects(Characters.Stats caster, Characters.Stats target) {
                return new SpellEffect[] {
                    new SpellEffectList.AddToModStat(target, StatType.HEALTH, -Util.Random(caster.GetStatCount(Value.MOD, StatType.STRENGTH) * CRITICAL_MULTIPLIER, .25f)),
                    new SpellEffectList.AddToModStat(caster, StatType.SKILL, 2)
                };
            }
        }

        public class Wait : SpellBook {
            public Wait() : base("Wait", "fox-head", TargetType.SELF, SpellType.MERCY, 0, 0) { }

            public override string CreateDescriptionHelper(Characters.Stats caster) {
                return "Literally do nothing.";
            }

            protected override IList<SpellEffect> GetHitEffects(Characters.Stats caster, Characters.Stats target) {
                return new SpellEffect[0];
            }
        }

        public class Poison : SpellBook {
            public Poison() : base("Poison", "fox-head", TargetType.SELF, SpellType.OFFENSE, 0, 0) { }

            public override string CreateDescriptionHelper(Characters.Stats caster) {
                return string.Format("Enemy is afflicted with Poison.");
            }

            protected override IList<SpellEffect> GetHitEffects(Characters.Stats caster, Characters.Stats target) {
                return new SpellEffect[] { new SpellEffectList.AddBuff(target, new BuffList.Poison(caster, target)) };
            }
        }
    }
}
