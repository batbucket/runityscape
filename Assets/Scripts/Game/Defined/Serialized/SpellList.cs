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

namespace Scripts.Game.Defined.Serialized.Spells {

    public class Attack : BasicSpellbook {
        public const float PERCENT = 0.01f;
        public const float CRITICAL_MULTIPLIER = 2f;
        public const float CRITICAL_VARIABILITY = 0.25f;
        public const float BASE_ACCURACY = .90f;
        public const float BASE_CRITICAL_RATE = .10f;
        public const int SKILL_ON_HIT = 1;
        public const int SKILL_ON_CRIT = 2;

        public Attack() : base("Attack", Util.GetSprite("fist"), TargetType.SINGLE_ENEMY, SpellType.OFFENSE) {
        }

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
            int lowerBound = (caster.Stats.GetStatCount(Stats.Get.MOD, StatType.STRENGTH) > 0) ? 1 : 0;

            return new SpellEffect[] {
                    new AddToModStat(target.Stats, StatType.HEALTH, -Util.RandomRange(lowerBound, caster.Stats.GetStatCount(Stats.Get.MOD, StatType.STRENGTH))), // Range(0, CurrentStrength)
                    new AddToModStat(caster.Stats, StatType.SKILL, 1)
                };
        }

        protected override IList<SpellEffect> GetCriticalEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] {
                    new AddToModStat(target.Stats, StatType.HEALTH, -Util.Random((int)(caster.Stats.GetStatCount(Stats.Get.MOD, StatType.STRENGTH) * CRITICAL_MULTIPLIER), CRITICAL_VARIABILITY)),
                    new AddToModStat(caster.Stats, StatType.SKILL, 2)
                };
        }
    }

    public class Wait : BasicSpellbook {
        public Wait() : base("Wait", Util.GetSprite("hourglass"), TargetType.SELF, SpellType.MERCY) {
            this.flags.Remove(Model.Spells.Flag.CASTER_REQUIRES_SPELL);
        }

        public override string CreateDescriptionHelper(SpellParams caster) {
            return "Literally do nothing.";
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[0];
        }
    }

    public class Check : BasicSpellbook {
        private static readonly Checked dummy = new Checked();

        public Check() : base("Check", Util.GetSprite("magnifying-glass"), TargetType.ANY, SpellType.BOOST) { }

        public override string CreateDescriptionHelper(SpellParams caster) {
            return BuffAdder.CreateBuffDescription(dummy);
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] {
                new PostText(CheckText(target)),
                new AddBuff(new BuffParams(caster.Stats, caster.CharacterId), target.Buffs, new Checked())
            };
        }

        private static string CheckText(SpellParams target) {
            string targetCheck = target.Look.Check;
            return string.Format(
                "{0}\n{1}{2}",
                target.Look.DisplayName,
                target.Stats.ShortAttributeDistribution,
                string.IsNullOrEmpty(targetCheck) ? string.Empty : string.Format("\n{0}", targetCheck)
                );
        }
    }

    public class InflictPoison : BuffAdder {
        public InflictPoison() : base(TargetType.SINGLE_ENEMY, SpellType.OFFENSE, new Poison(), "Infect") {
            AddCost(StatType.MANA, 1);
        }
    }

    public class SetupCounter : BuffAdder {
        public SetupCounter() : base(TargetType.SELF, SpellType.DEFENSE, new Counter(), "Counter") {
            AddCost(StatType.SKILL, 2);
        }
    }

    public class Heal : BasicSpellbook {
        public Heal() : base("Heal", Util.GetSprite("health-normal"), TargetType.SINGLE_ALLY, SpellType.BOOST) { }

        public override string CreateDescriptionHelper(SpellParams caster) {
            return "Restore a little health.";
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] {
                new AddToModStat(target.Stats, StatType.HEALTH, 2)
            };
        }
    }

    public class ReflectiveClone : BasicSpellbook {
        private const int NUMBER_OF_CLONES = 2;

        public ReflectiveClone() : base("Trickster Art: Reflective Clones", Util.GetSprite("fox-head"), TargetType.SELF, SpellType.BOOST) { }

        public override string CreateDescriptionHelper(SpellParams caster) {
            return "Creates clones of the caster that vanish when the caster is attacked. Clones reflect attacks. Shuffles the caster's side.";
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            Func<Character> cloneFunc =
                () => {
                    Character c = new Character(
                        new Stats(caster.Stats.Level, 0, 1, 1, 1),
                        new Look(caster.Look.Name, caster.Look.Sprite, caster.Look.Tooltip, caster.Look.Check, caster.Look.Breed, caster.Look.TextColor),
                        new RuinsBrains.KitsuneClone());
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

        public override string CreateDescriptionHelper(SpellParams caster) {
            return string.Format("Escape back to {0}.", destination.Location);
        }

        protected override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] { new GoToPage(destination, stopBattle) };
        }
    }
}