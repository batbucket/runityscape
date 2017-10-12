using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Model.Stats;
using Scripts.View.Portraits;
using Scripts.Model.Spells;
using Scripts.Game.Defined.SFXs;
using Scripts.Model.Characters;
using Scripts.Game.Defined.Spells;
using Scripts.Model.Buffs;
using Scripts.Model.Pages;
using Scripts.View.Effects;
using Scripts.Game.Serialized.Brains;
using UnityEngine;
using Scripts.Presenter;
using Scripts.Game.Defined.Characters;
using Scripts.Game.Defined.Serialized.Buffs;
using Scripts.Game.Defined.Unserialized.Spells;
using Scripts.Game.Defined.Unserialized.Buffs;

namespace Scripts.Game.Defined.Serialized.Spells {

    public class Attack : BasicSpellbook {
        public const float PERCENT = 0.01f;
        public const float HIT_VARIANCE = .25f;
        public const float HIT_STRENGTH_RATIO = 0.5f;
        public const float CRITICAL_STRENGTH_RATIO = 1f;
        public const float CRITICAL_VARIANCE = .125f;
        public const float BASE_ACCURACY = .90f;
        public const float BASE_CRITICAL_RATE = 0f;
        public const int SKILL_ON_HIT = 1;
        public const int SKILL_ON_CRIT = 2;

        public Attack() : base("Attack", Util.GetSprite("fist"), TargetType.ONE_FOE, SpellType.OFFENSE) {
        }

        protected override string CreateDescriptionHelper() {
            return string.Format("A basic attack with the weapons or fists.");
        }

        protected override IList<IEnumerator> GetHitSFX(Character caster, Character target) {
            return new IEnumerator[] { SFX.DoMeleeEffect(caster, target, 0.5f, "Slash_0") };
        }

        protected override bool IsHit(Character caster, Character target) {
            return Util.IsChance(BASE_ACCURACY + StatUtil.GetDifference(StatType.AGILITY, caster.Stats, target.Stats) * PERCENT);
        }

        protected override bool IsCritical(Character caster, Character target) {
            return Util.IsChance(BASE_CRITICAL_RATE + StatUtil.GetDifference(StatType.AGILITY, caster.Stats, target.Stats) * PERCENT);
        }

        protected override IList<SpellEffect> GetHitEffects(Page page, Character caster, Character target) {
            int totalCasterStrength = caster.Stats.GetStatCount(Stats.Get.TOTAL, StatType.STRENGTH);
            int damage = -Util.Random(Mathf.CeilToInt(totalCasterStrength * HIT_STRENGTH_RATIO), HIT_VARIANCE);

            return new SpellEffect[] {
                    new AddToModStat(target.Stats, StatType.HEALTH, damage),
                    new AddToModStat(caster.Stats, StatType.SKILL, 1)
                };
        }

        protected override IList<SpellEffect> GetCriticalEffects(Page page, Character caster, Character target) {
            int totalCasterStrength = caster.Stats.GetStatCount(Stats.Get.TOTAL, StatType.STRENGTH);
            int damage = -Util.Random(totalCasterStrength, CRITICAL_VARIANCE);
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

        protected override IList<SpellEffect> GetHitEffects(Page page, Character caster, Character target) {
            return new SpellEffect[0];
        }
    }

    public class Check : BasicSpellbook {
        private static readonly Checked DUMMY = new Checked();

        public Check() : base("Check", Util.GetSprite("magnifying-glass"), TargetType.ANY, SpellType.BOOST) {
            AddCost(StatType.MANA, 10);
        }

        protected override string CreateDescriptionHelper() {
            return BuffAdder.CreateBuffDescription(this.TargetType, DUMMY);
        }

        protected override IList<SpellEffect> GetHitEffects(Page page, Character caster, Character target) {
            return new SpellEffect[] {
                new PostText(CheckText(target)),
                new AddBuff(new BuffParams(caster.Stats, caster.Id), target.Buffs, new Checked())
            };
        }

        private static string CheckText(Character target) {
            return string.Format(
                "{0}\n{1}",
                target.Look.DisplayName,
                target.Stats.ShortAttributeDistribution
                );
        }
    }

    public class InflictPoison : BuffAdder {

        public InflictPoison() : base(TargetType.ONE_FOE, SpellType.OFFENSE, new Poison(), "Infect", PriorityType.NORMAL) {
            AddCost(StatType.MANA, 1);
        }
    }

    public class CrushingBlow : BasicSpellbook {
        private const int INTELLECT_TO_DAMAGE_RATIO = 1;

        public CrushingBlow() : base("Crushing Blow", Util.GetSprite("fist"), TargetType.ONE_FOE, SpellType.OFFENSE, PriorityType.LOW) {
            AddCost(StatType.SKILL, 2);
        }

        protected override string CreateDescriptionHelper() {
            return "A powerful blow that occurs slower than other spells.";
        }

        protected override IList<SpellEffect> GetHitEffects(Page page, Character caster, Character target) {
            return new SpellEffect[] {
                    new AddToModStat(
                        target.Stats,
                        StatType.HEALTH,
                        caster.Stats.GetStatCount(
                            Stats.Get.TOTAL,
                            StatType.INTELLECT) * -INTELLECT_TO_DAMAGE_RATIO)
                };
        }

        protected override IList<IEnumerator> GetHitSFX(Character caster, Character target) {
            return new IEnumerator[] {
                SFX.DoMeleeEffect(caster, target, 0.5f, "Boom_6")
            };
        }
    }

    public class PlayerHeal : BasicSpellbook {
        private const int INTELLECT_TO_HEALTH = 1;
        private const int CRITICAL_INTELLECT_TO_HEALTH = 2;
        private const int CRITICAL_HEALTH_PERCENT = 10;

        public PlayerHeal() : base("Heal", Util.GetSprite("health-normal"), TargetType.ONE_ALLY, SpellType.BOOST) {
            AddCost(StatType.MANA, 20);
        }

        protected override string CreateDescriptionHelper() {
            return string.Format(
                "Restore {0} to a single ally. Has an increased effect on targets below {1}% {0}.",
                StatType.HEALTH.ColoredName,
                CRITICAL_HEALTH_PERCENT);
        }

        protected override IList<SpellEffect> GetHitEffects(Page page, Character caster, Character target) {
            return new SpellEffect[] {
                new AddToModStat(target.Stats, StatType.HEALTH, caster.Stats.GetStatCount(Stats.Get.TOTAL, StatType.INTELLECT) * INTELLECT_TO_HEALTH)
            };
        }

        protected override IList<IEnumerator> GetHitSFX(Character caster, Character target) {
            return new IEnumerator[] { SFX.PlaySound("healspell1") };
        }

        protected override bool IsCritical(Character caster, Character target) {
            float currentHealth = target.Stats.GetStatCount(Stats.Get.MOD, StatType.HEALTH);
            float maxHealth = target.Stats.GetStatCount(Stats.Get.MAX, StatType.HEALTH);

            return (currentHealth / maxHealth) <= (CRITICAL_HEALTH_PERCENT / 100f);
        }

        protected override IList<SpellEffect> GetCriticalEffects(Page page, Character caster, Character target) {
            return new SpellEffect[] {
                new AddToModStat(target.Stats, StatType.HEALTH, caster.Stats.GetStatCount(Stats.Get.TOTAL, StatType.INTELLECT) * CRITICAL_INTELLECT_TO_HEALTH)
            };
        }
    }

    public class SetupDefend : BuffAdder {

        public SetupDefend() : base(TargetType.SELF, SpellType.DEFENSE, new Defend(), "Defend", PriorityType.HIGHEST) {
        }

        protected override IList<IEnumerator> GetHitSFX(Character caster, Character target) {
            return new IEnumerator[] { SFX.PlaySound("ping") };
        }
    }

    public class Arraystrike : BasicSpellbook {

        public Arraystrike() : base("Arraystrike", Util.GetSprite("sword-array"), TargetType.ALL_FOE, SpellType.OFFENSE, PriorityType.LOW) {
            AddCost(StatType.SKILL, 5);
        }

        protected override string CreateDescriptionHelper() {
            return string.Format("A delayed strike that hits all enemies.");
        }

        protected override IList<SpellEffect> GetHitEffects(Page page, Character caster, Character target) {
            int damage = -caster.Stats.GetStatCount(Stats.Get.MOD, StatType.INTELLECT);
            return new SpellEffect[] {
                new AddToModStat(target.Stats, StatType.HEALTH, damage),
            };
        }

        protected override IList<IEnumerator> GetHitSFX(Character caster, Character target) {
            return new IEnumerator[] {
                SFX.DoMeleeEffect(caster, target, 0.2f, "Slash_0")
            };
        }
    }
}

/// <summary>
/// Spells that only enemies can have don't need to be saved.
/// </summary>
namespace Scripts.Game.Defined.Unserialized.Spells {

    public class SetupCounter : BuffAdder {

        public SetupCounter() : base(TargetType.SELF, SpellType.DEFENSE, new Counter(), "Counter", PriorityType.LOW) {
            AddCost(StatType.SKILL, 3);
        }
    }

    public class EnemyHeal : BasicSpellbook {
        private const int BASE_HEALING_AMOUNT = 10;

        public EnemyHeal() : base("Heal", Util.GetSprite("health-normal"), TargetType.ONE_ALLY, SpellType.BOOST, PriorityType.HIGH) {
        }

        protected override string CreateDescriptionHelper() {
            return "Restore a little health.";
        }

        protected override IList<SpellEffect> GetHitEffects(Page page, Character caster, Character target) {
            return new SpellEffect[] {
                new AddToModStat(target.Stats, StatType.HEALTH, BASE_HEALING_AMOUNT + caster.Stats.GetStatCount(Stats.Get.TOTAL, StatType.INTELLECT))
            };
        }

        protected override IList<IEnumerator> GetHitSFX(Character caster, Character target) {
            return new IEnumerator[] { SFX.PlaySound("healspell1")
                };
        }
    }

    public class RevealTrueForm : BasicSpellbook {

        public RevealTrueForm() : base("True Form", Util.GetSprite("paranoia"), TargetType.SELF, SpellType.BOOST) {
        }

        protected override string CreateDescriptionHelper() {
            return "Reveal your true form.";
        }

        protected override IList<IEnumerator> GetHitSFX(Character caster, Character target) {
            return new IEnumerator[] {
                    SFX.DoSteamEffect(caster),
                    SFX.PlaySound("Creepy"),
                    SFX.PlaySound("i_see_you_voice"),
                    SFX.LoopMusic("enchanted tiki 86")
                };
        }

        protected override IList<SpellEffect> GetHitEffects(Page page, Character caster, Character target) {
            return new SpellEffect[] {
                new ChangeLookEffect(caster, RuinsNPCs.ReplicantLook()),
            };
        }
    }

    public class ReflectiveClone : BasicSpellbook {
        private const int NUMBER_OF_CLONES = 4;

        public ReflectiveClone() : base("Hallucination", Util.GetSprite("fox-head"), TargetType.SELF, SpellType.BOOST, PriorityType.LOWEST) {
        }

        protected override string CreateDescriptionHelper() {
            return "Creates clones of the caster. Clones do no damage, and die when the caster is attacked. Clones reflect all damage taken.";
        }

        protected override IList<IEnumerator> GetHitSFX(Character caster, Character target) {
            return new IEnumerator[] {
                SFX.Wait(HitsplatView.TOTAL_DURATION),
                SFX.PlaySound("synthetic_explosion_1")
            };
        }

        protected override IList<SpellEffect> GetHitEffects(Page current, Character caster, Character target) {
            Func<Character> cloneFunc =
                () => {
                    Character c = new Character(
                        new Stats(caster.Stats.Level, 0, caster.Stats.GetStatCount(Stats.Get.TOTAL, StatType.AGILITY), 1, 1),
                        new Look(caster.Look.Name, caster.Look.Sprite, caster.Look.Tooltip, caster.Look.Breed, caster.Look.TextColor),
                        new ReplicantClone());
                    c.AddFlag(Model.Characters.Flag.IS_CLONE);
                    c.Buffs.AddBuff(new ReflectAttack(), c);
                    c.Buffs.AddBuff(new SpiritLink(), caster);
                    return c;
                };

            return new SpellEffect[] {
                new CloneEffect(NUMBER_OF_CLONES, current.GetSide(caster), cloneFunc, current),
                new ShuffleEffect(current, current.GetSide(caster))
            };
        }
    }

    public class Blackout : BuffAdder {
        private static readonly BlackedOut DUMMY = new BlackedOut();

        public Blackout() : base(TargetType.ONE_FOE, SpellType.OFFENSE, DUMMY, "Blackout", PriorityType.HIGH) {
        }
    }

    public class Ignite : BuffAdder {
        private static readonly Ignited DUMMY = new Ignited();

        public Ignite() : base(TargetType.ONE_FOE, SpellType.OFFENSE, DUMMY, "Ignite", PriorityType.NORMAL) {
            AddCost(StatType.MANA, 10);
        }

        protected override IList<IEnumerator> GetHitSFX(Character caster, Character target) {
            return new IEnumerator[] {
                SFX.PlaySound("synthetic_explosion_1"),
                SFX.DoSteamEffect(target, Color.red)
            };
        }
    }

    public class SingStrengthSong : SingSirenSong {

        public SingStrengthSong() : base(new StrengthSirenSong()) {
        }
    }

    public class SingAgilitySong : SingSirenSong {

        public SingAgilitySong() : base(new AgilitySirenSong()) {
        }
    }

    public class SingIntellectSong : SingSirenSong {

        public SingIntellectSong() : base(new IntellectSirenSong()) {
        }
    }

    public class SingVitalitySong : SingSirenSong {

        public SingVitalitySong() : base(new VitalitySirenSong()) {
        }
    }

    public class SpawnTentacles : BasicSpellbook {

        public SpawnTentacles() : base("Tentacle Eruption", Util.GetSprite("shark"), TargetType.SELF, SpellType.BOOST) {
            AddCost(StatType.SKILL, Kraken.TURNS_BETWEEN_TENTACLE_SUMMONS);
        }

        protected override string CreateDescriptionHelper() {
            return string.Format("Spawns Tentacles to defend the caster. Tentacles intercept all attacks. Number increases with missing health.");
        }

        protected override IList<SpellEffect> GetHitEffects(Page page, Character caster, Character target) {
            float targetHealthPercentage = ((float)target.Stats.GetStatCount(Stats.Get.MOD, StatType.HEALTH)) / target.Stats.GetStatCount(Stats.Get.MAX, StatType.HEALTH);
            int tentaclesToSummon = 0;

            // TODO fix this terrible code, use a dictionary please
            if (targetHealthPercentage > .66) {
                tentaclesToSummon = 2;
            } else if (targetHealthPercentage > .33) {
                tentaclesToSummon = 3;
            } else {
                tentaclesToSummon = 4;
            }

            Func<Character> summonTentacleFunc = () => {
                Character tentacle = OceanNPCs.Tentacle();
                Interceptor interceptor = new Interceptor();
                interceptor.Caster = new BuffParams(target.Stats, target.Id);
                tentacle.Buffs.AddBuff(interceptor);
                return tentacle;
            };
            return new SpellEffect[] {
                new SummonEffect(page.GetSide(target), page, summonTentacleFunc, tentaclesToSummon)
            };
        }
    }

    public abstract class SingSirenSong : BuffAdder {

        public SingSirenSong(Buff sirenSong) : base(TargetType.ALL_FOE, SpellType.OFFENSE, sirenSong, Util.GetSprite("sonic-shout")) {
        }
    }

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

        protected override IList<SpellEffect> GetHitEffects(Page current, Character caster, Character target) {
            return new SpellEffect[] { new GoToPage(destination, stopBattle) };
        }
    }

    public class WaterboltSingle : WaterboltAbstract {
        private const string SINGLE_TARGET_DESCRIPTION = "A burst of boiling water that has a chance to inflict\n<color=cyan>{0}</color>\n{1}";

        public WaterboltSingle() : base(TargetType.ONE_FOE, "Scald", SINGLE_TARGET_DESCRIPTION) {
        }
    }

    public class WaterboltMulti : WaterboltAbstract {
        private const int MANA_COST = 10;
        private const string MULTI_TARGET_DESCRIPTION = "Bursts of boiling water that have a chance to inflict\n<color=cyan>{0}</color>\n{1}";

        public WaterboltMulti() : base(TargetType.ALL_FOE, "Multi Scald", MULTI_TARGET_DESCRIPTION) {
            AddCost(StatType.MANA, MANA_COST);
        }
    }

    public abstract class WaterboltAbstract : BasicSpellbook {
        private const float CHANCE_OF_CRITICAL = 0.30f;
        private static readonly Buff IGNITE_BUFF = new Ignited();

        private readonly string description;

        public WaterboltAbstract(TargetType targetType, string name, string description) : base(name, Util.GetSprite("water-bolt"), targetType, SpellType.OFFENSE) {
            this.description = description;
        }

        protected override string CreateDescriptionHelper() {
            return string.Format(
                description,
                IGNITE_BUFF.Name,
                IGNITE_BUFF.Description);
        }

        protected override bool IsCritical(Character caster, Character target) {
            return Util.IsChance(CHANCE_OF_CRITICAL);
        }

        protected override IList<SpellEffect> GetCriticalEffects(Page page, Character caster, Character target) {
            return new SpellEffect[] {
                DamageEffect(caster, target),
                new AddBuff(new BuffParams(caster.Stats, caster.Id), target.Buffs, new Ignited())
            };
        }

        protected override IList<SpellEffect> GetHitEffects(Page page, Character caster, Character target) {
            return new SpellEffect[] {
                DamageEffect(caster, target)
            };
        }

        protected override IList<IEnumerator> GetHitSFX(Character caster, Character target) {
            return new IEnumerator[] {
                SFX.DoSteamEffect(target, Color.cyan)
            };
        }

        private SpellEffect DamageEffect(Character caster, Character target) {
            return new AddToModStat(target.Stats, StatType.HEALTH, -caster.Stats.GetStatCount(Stats.Get.TOTAL, StatType.INTELLECT));
        }
    }
}