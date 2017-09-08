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
using UnityEngine;
using Scripts.Model.Pages;
using Scripts.Game.Defined.Serialized.Characters;
using Scripts.View.Effects;

namespace Scripts.Game.Defined.Serialized.Spells {

    public class Attack : BasicSpellbook {
        public const float PERCENT = 0.01f;
        public const float CRITICAL_MULTIPLIER = 1f;
        public const float CRITICAL_VARIABILITY = 0f;
        public const float BASE_ACCURACY = .90f;
        public const float BASE_CRITICAL_RATE = .10f;
        public const int SKILL_ON_HIT = 1;
        public const int SKILL_ON_CRIT = 2;

        public Attack() : base("Attack", Util.GetSprite("fist"), TargetType.SINGLE_ENEMY, SpellType.OFFENSE) {
        }

        protected override string CreateDescriptionHelper() {
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
            int damage = 0;
            if (caster.Stats.GetStatCount(Stats.Get.MOD_AND_EQUIP, StatType.STRENGTH) > 0) {
                int lowerBound = (caster.Stats.GetStatCount(Stats.Get.MOD_AND_EQUIP, StatType.STRENGTH) > 0) ? 1 : 0;
                damage = -Util.RandomRange(lowerBound, caster.Stats.GetStatCount(Stats.Get.MOD_AND_EQUIP, StatType.STRENGTH));
            }
            return new SpellEffect[] {
                    new AddToModStat(target.Stats, StatType.HEALTH, damage), // Range(0, CurrentStrength)
                    new AddToModStat(caster.Stats, StatType.SKILL, 1)
                };
        }

        protected override IList<SpellEffect> GetCriticalEffects(SpellParams caster, SpellParams target) {
            int damage = 0;
            if (caster.Stats.GetStatCount(Stats.Get.MOD_AND_EQUIP, StatType.STRENGTH) > 0) {
                damage = -Util.Random(((int)(caster.Stats.GetStatCount(Stats.Get.MOD_AND_EQUIP, StatType.STRENGTH) * CRITICAL_MULTIPLIER)), CRITICAL_VARIABILITY);
            }
            return new SpellEffect[] {
                    new AddToModStat(target.Stats, StatType.HEALTH, damage),
                    new AddToModStat(caster.Stats, StatType.SKILL, 2)
                };
        }
    }

    public class Wait : BasicSpellbook {
        public Wait() : base("Wait", Util.GetSprite("hourglass"), TargetType.SELF, SpellType.MERCY) {
            this.flags.Remove(Model.Spells.Flag.CASTER_REQUIRES_SPELL);
        }

        protected override string CreateDescriptionHelper() {
            return "Literally do nothing.";
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[0];
        }
    }

    public class Check : BasicSpellbook {
        private static readonly Checked DUMMY = new Checked();

        public Check() : base("Check", Util.GetSprite("magnifying-glass"), TargetType.ANY, SpellType.BOOST) { }

        protected override string CreateDescriptionHelper() {
            return BuffAdder.CreateBuffDescription(DUMMY);
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] {
                new PostText(CheckText(target)),
                new AddBuff(new BuffParams(caster.Stats, caster.CharacterId), target.Buffs, new Checked())
            };
        }

        private static string CheckText(SpellParams target) {
            return string.Format(
                "{0}\n{1}",
                target.Look.DisplayName,
                target.Stats.ShortAttributeDistribution
                );
        }
    }

    public class InflictPoison : BuffAdder {
        public InflictPoison() : base(TargetType.SINGLE_ENEMY, SpellType.OFFENSE, new Poison(), "Infect", PriorityType.NORMAL) {
            AddCost(StatType.MANA, 1);
        }
    }

    public class SetupCounter : BuffAdder {
        public SetupCounter() : base(TargetType.SELF, SpellType.DEFENSE, new Counter(), "Counter", PriorityType.LOW) {
            AddCost(StatType.SKILL, 3);
        }
    }

    public class Heal : BasicSpellbook {
        private const int HEALING_AMOUNT = 10;

        public Heal() : base("Heal", Util.GetSprite("health-normal"), TargetType.SINGLE_ALLY, SpellType.BOOST, PriorityType.HIGH) { }

        protected override string CreateDescriptionHelper() {
            return "Restore a little health.";
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] {
                new AddToModStat(target.Stats, StatType.HEALTH, HEALING_AMOUNT)
            };
        }

        protected override IList<IEnumerator> GetHitSFX(PortraitView caster, PortraitView target) {
            return new IEnumerator[] { SFX.PlaySound("healspell1")
                };
        }
    }

    public class ReflectiveClone : BasicSpellbook {
        private const int NUMBER_OF_CLONES = 4;

        public ReflectiveClone() : base("Harmless Illusions", Util.GetSprite("fox-head"), TargetType.SELF, SpellType.BOOST, PriorityType.LOW) { }

        protected override string CreateDescriptionHelper() {
            return "Creates clones of the caster that die when the caster is attacked. Clones reflect attacks and do no damage.";
        }

        protected override IList<IEnumerator> GetHitSFX(PortraitView caster, PortraitView target) {
            return new IEnumerator[] {
                SFX.Wait(HitsplatView.TOTAL_DURATION),
                SFX.PlaySound("synthetic_explosion_1")
            };
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            Func<Character> cloneFunc =
                () => {
                    Character c = new Character(
                        new Stats(caster.Stats.Level, 0, caster.Stats.GetStatCount(Stats.Get.MOD_AND_EQUIP, StatType.AGILITY), 1, 1),
                        new Look(caster.Look.Name, caster.Look.Sprite, caster.Look.Tooltip, caster.Look.Breed, caster.Look.TextColor),
                        new FieldBrains.ReplicantClone());
                    c.AddFlag(Model.Characters.Flag.IS_CLONE);
                    c.Buffs.AddBuff(new ReflectAttack(), c);
                    c.Buffs.AddBuff(new SpiritLink(), caster.Character);
                    return c;
                };

            return new SpellEffect[] {
                new CloneEffect(NUMBER_OF_CLONES, caster.Page.GetSide(caster.Character), cloneFunc, caster.Page),
                new ShuffleEffect(caster.Page, caster.Page.GetSide(caster.Character))
            };
        }
    }

    public class Blackout : BuffAdder {
        private static readonly BlackedOut DUMMY = new BlackedOut();

        public Blackout() : base(TargetType.SINGLE_ENEMY, SpellType.OFFENSE, DUMMY, "Blackout", PriorityType.HIGH) {

        }
    }
}

namespace Scripts.Game.Defined.Unserialized.Spells {
    public class Flee : BasicSpellbook {
        private Page destination;
        private Action stopBattle;

        public Flee(Page destination, Action stopBattle) : base("Flee", Util.GetSprite("walking-boot"), TargetType.SELF, SpellType.DEFENSE) {
            this.destination = destination;
            this.stopBattle = stopBattle;
            this.flags.Remove(Model.Spells.Flag.CASTER_REQUIRES_SPELL);
        }

        protected override string CreateDescriptionHelper() {
            return string.Format("Escape back to {0}.", destination.Location);
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] { new GoToPage(destination, stopBattle) };
        }
    }
}