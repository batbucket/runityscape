using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Game.Defined.Spells;
using Scripts.Game.Defined.Unserialized.Buffs;
using Scripts.Model.Buffs;
using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Defined.Serialized.Buffs {

    public class FishShook : Buff {
        private const float FISHY_TARGET_MULTIPLIER = 2;
        private const float OTHER_TARGET_MULTIPLIER = 0.25f;

        public FishShook()
            : base(Util.GetSprite("fish"),
                  "Fish-Shook",
                  string.Format(
                      "Basic attacks do {0}x damage on fishy targets and {1} damage on non-fish.", FISHY_TARGET_MULTIPLIER, OTHER_TARGET_MULTIPLIER), false) {
        }

        public override bool IsReact(SingleSpell spellToReactTo, Stats owner) {
            return spellToReactTo.SpellBook is Attack && spellToReactTo.Result.IsDealDamage && spellToReactTo.Caster.Stats == owner;
        }

        protected override void ReactHelper(SingleSpell spellToReactTo, Stats owner) {
            float localDmgMult = 0;

            // is a fish
            if (spellToReactTo.Target.Look.Breed == Characters.Breed.FISH) {
                localDmgMult = FISHY_TARGET_MULTIPLIER;
            } else { // not a fish
                localDmgMult = OTHER_TARGET_MULTIPLIER;
            }

            SpellEffect healthDamage = null;
            for (int i = 0; (i < spellToReactTo.Result.Effects.Count) && (healthDamage == null); i++) {
                SpellEffect se = spellToReactTo.Result.Effects[i];
                AddToModStat addToModStat = se as AddToModStat;
                if (addToModStat != null && addToModStat.AffectedStat == StatType.HEALTH && addToModStat.Value < 0) {
                    healthDamage = addToModStat;
                }
            }
            if (healthDamage != null) {
                healthDamage.Value = (int)Math.Floor(healthDamage.Value * localDmgMult);
            }
        }
    }

    public class DamageResist : Buff {
        private const float DAMAGE_MULTIPLIER = 0.7f;

        public DamageResist()
            : base(Util.GetSprite("round-shield"),
                  "Damage Resist",
                  String.Format("Reduces incident damage from basic attacks by {0}%.",
                      (1 - DAMAGE_MULTIPLIER) * 100), false) { }

        protected override void ReactHelper(SingleSpell spellToReactTo, Stats owner) {
            SpellEffect healthDamage = null;
            for (int i = 0; (i < spellToReactTo.Result.Effects.Count) && (healthDamage == null); i++) {
                SpellEffect se = spellToReactTo.Result.Effects[i];
                AddToModStat addToModStat = se as AddToModStat;
                if (addToModStat != null && addToModStat.AffectedStat == StatType.HEALTH && addToModStat.Value < 0) {
                    healthDamage = addToModStat;
                }
            }
            if (healthDamage != null) {
                int tempValue = healthDamage.Value;
                healthDamage.Value = (int)Math.Floor(healthDamage.Value * DAMAGE_MULTIPLIER);
            }
        }

        public override bool IsReact(SingleSpell spellToReactTo, Stats owner) {
            return spellToReactTo.SpellBook is Attack && spellToReactTo.Target.Stats == owner;
        }
    }

    public class Ignited : Buff {
        private static readonly StatType REDUCED_STAT = StatType.STRENGTH;
        private const int STAT_REDUCTION_PERCENT = 20;
        private const int DAMAGE_PER_TURN = 1;
        private const int DURATION = 5;

        public Ignited()
            : base(DURATION,
                  Util.GetSprite("fire"),
                  "Ignited",
                  string.Format("{0} reduced. Take {1} damage at end of turn.",
                      REDUCED_STAT.ColoredName,
                      DAMAGE_PER_TURN), true) {
            AddMultiplicativeStatBonus(REDUCED_STAT, -STAT_REDUCTION_PERCENT);
        }

        protected override IList<SpellEffect> OnEndOfTurnHelper(Stats owner) {
            return new SpellEffect[] {
                new AddToModStat(owner, StatType.HEALTH, -DAMAGE_PER_TURN)
            };
        }
    }

    public class StrengthBoost : Buff {
        public const int STRENGTH_INCREASE_AMOUNT = 50;

        public StrengthBoost()
            : base(Util.GetSprite("fist"),
                  "Strength boost",
                  "Increases strength.",
                  true) {
            AddMultiplicativeStatBonus(StatType.STRENGTH, STRENGTH_INCREASE_AMOUNT);
        }
    }

    public class Insight : Buff {
        public const int MANA_RECOVERED_PER_TURN = 5;

        public Insight() : base(Util.GetSprite("water-drop"), "Insight", string.Format("Regenerating {0}.", StatType.MANA.ColoredName), false) {
        }

        protected override IList<SpellEffect> OnEndOfTurnHelper(Stats owner) {
            return new SpellEffect[] {
                new AddToModStat(owner, StatType.MANA, MANA_RECOVERED_PER_TURN)
            };
        }
    }

    public class Poison : Buff {

        public Poison() : base(2, Util.GetSprite("fox-head"), "Poisoned", "Loses health at the end of each turn.", true) {
        }

        protected override IList<SpellEffect> OnEndOfTurnHelper(Model.Characters.Stats owner) {
            return new SpellEffect[] {
                    new AddToModStat(owner, StatType.HEALTH, -1)
                };
        }
    }

    public class StrengthScalingPoison : Buff {

        public StrengthScalingPoison() : base(2, Util.GetSprite("fox-head"), "StrengthScalingPoison", "Loses health at the end of each turn.", true) {
        }

        protected override IList<SpellEffect> OnEndOfTurnHelper(Model.Characters.Stats owner) {
            return new SpellEffect[] {
                new AddToModStat(owner, StatType.HEALTH, BuffCaster.GetStatCount(Stats.Get.TOTAL, StatType.STRENGTH))
            };
        }
    }

    public class Checked : Buff {

        public Checked() : base(5, Util.GetSprite("magnifying-glass"), "Checked", "Resource visibility increased.", true) {
        }

        protected override IList<SpellEffect> OnApplyHelper(Stats owner) { // TODO UNSTACKABLE
            return new SpellEffect[] { new AddToResourceVisibility(owner, 1) };
        }

        protected override IList<SpellEffect> OnTimeOutHelper(Stats owner) {
            return new SpellEffect[] { new AddToResourceVisibility(owner, -1) };
        }
    }

    public class BlackedOut : Buff {

        public BlackedOut() : base(3, Util.GetSprite("sight-disabled"), "Blackout", "Resource visibility decreased.", true) {
        }

        protected override IList<SpellEffect> OnApplyHelper(Stats owner) {
            return new SpellEffect[] { new AddToResourceVisibility(owner, -1) };
        }

        protected override IList<SpellEffect> OnTimeOutHelper(Stats owner) {
            return new SpellEffect[] { new AddToResourceVisibility(owner, 1) };
        }
    }

    public class Counter : Buff {
        private int DAMAGE_RATIO_FROM_ATTACK = 2;

        public Counter() : base(2, Util.GetSprite("round-shield"), "Counter", "Basic <color=yellow>Attack</color>s on this unit are reflected.", false) {
        }

        public override bool IsReact(SingleSpell spellToReactTo, Stats owner) {
            return spellToReactTo.SpellBook is Attack && spellToReactTo.Target.Stats == owner;
        }

        protected override void ReactHelper(SingleSpell spellToReactTo, Stats owner) {
            spellToReactTo.Result.Effects.Clear();
            spellToReactTo.Result.AddEffect(
                new AddToModStat(
                    spellToReactTo.Caster.Stats, StatType.HEALTH,
                    -spellToReactTo.Caster.Stats.GetStatCount(Stats.Get.MOD, StatType.STRENGTH) * DAMAGE_RATIO_FROM_ATTACK)
                );
        }
    }

    public class Defend : Buff {
        private const int DAMAGE_REDUCTION_PERCENT = 75;
        private const float DAMAGE_REDUCTION = DAMAGE_REDUCTION_PERCENT / 100f;

        public Defend()
            : base(1,
                  Util.GetSprite("shield"),
                  "Guard",
                  string.Format("Reduces damage taken from spells this turn by {0}%.", DAMAGE_REDUCTION_PERCENT),
                  false) {
        }

        public override bool IsReact(SingleSpell spellToReactTo, Stats owner) {
            return spellToReactTo.Target.Stats == owner && spellToReactTo.Result.IsDealDamage;
        }

        protected override void ReactHelper(SingleSpell spellToReactTo, Stats owner) {
            foreach (SpellEffect se in spellToReactTo.Result.Effects) {
                AddToModStat addToModStat = se as AddToModStat;
                if (addToModStat != null && addToModStat.AffectedStat == StatType.HEALTH && addToModStat.Value < 0) {
                    addToModStat.Value = (int)(addToModStat.Value * DAMAGE_REDUCTION);
                }
            }
        }
    }

    public class RegenerateHealth : StatRegen {

        public RegenerateHealth() : base(StatType.HEALTH, 1) {
        }
    }

    public class RegenerateMana : StatRegen {

        public RegenerateMana() : base(StatType.MANA, 2) {
        }
    }

    public class StrengthSirenSong : SirenSong {

        public StrengthSirenSong() : base(StatType.STRENGTH, "Lyric of Lethargy") {
        }
    }

    public class AgilitySirenSong : SirenSong {

        public AgilitySirenSong() : base(StatType.AGILITY, "Shanty of Snails") {
        }
    }

    public class IntellectSirenSong : SirenSong {

        public IntellectSirenSong() : base(StatType.INTELLECT, "Hymn of Horror") {
        }
    }

    public class VitalitySirenSong : SirenSong {

        public VitalitySirenSong() : base(StatType.VITALITY, "Melody of Mortality") {
        }
    }

    public class DelayedDeath : DelayedEffectSong {
        private static readonly StatType AFFECTED_STAT = StatType.HEALTH;

        public DelayedDeath() : base(4, "skull-crack", AFFECTED_STAT, "Curse: Death") {
        }
    }

    public class DelayedHyperDeath : DelayedEffectSong {
        private static readonly StatType AFFECTED_STAT = StatType.VITALITY;

        public DelayedHyperDeath() : base(4, "skull-crossed-bones", AFFECTED_STAT, "Curse: Hyperdeath") {
        }
    }
}

namespace Scripts.Game.Defined.Unserialized.Buffs {

    public abstract class DelayedEffectSong : Buff {
        private readonly StatType affectedStat;

        public DelayedEffectSong(int duration, string spriteLoc, StatType affectedStat, string name)
            : base(duration,
                  Util.GetSprite(spriteLoc),
                  name,
                  string.Format("On time-out, unit loses ALL {0}.", affectedStat.ColoredName),
                  true) {
            this.affectedStat = affectedStat;
        }

        protected override IList<SpellEffect> OnDispellHelper(Stats ownerOfThisBuff) {
            return new SpellEffect[0];
        }

        protected override IList<SpellEffect> OnTimeOutHelper(Stats ownerOfThisBuff) {
            return new SpellEffect[] {
                new AddToModStat(ownerOfThisBuff, affectedStat, -ownerOfThisBuff.GetStatCount(Stats.Get.TOTAL, affectedStat))
            };
        }
    }

    public class SpiritLink : Buff {

        public SpiritLink() : base(Util.GetSprite("knot"), "Spirit Link", "Attacks on the non-clone unit will also cause this unit to take damage.", false) {
        }

        /// <summary>
        /// The caster of the buff (the non-clone) is hit by an offensive spell that deals damage.
        /// </summary>
        public override bool IsReact(SingleSpell spellToReactTo, Stats buffHolder) {
            return spellToReactTo.SpellBook.SpellType == SpellType.OFFENSE
                && spellToReactTo.Target.Stats == this.BuffCaster
                && spellToReactTo.Result.Type.IsSuccessfulType
                && spellToReactTo.Result.IsDealDamage;
        }

        protected override void ReactHelper(SingleSpell spellToReactTo, Stats buffHolder) {
            SpellEffect healthDamage = null;
            for (int i = 0; (i < spellToReactTo.Result.Effects.Count) && (healthDamage == null); i++) {
                SpellEffect se = spellToReactTo.Result.Effects[i];
                AddToModStat addToModStat = se as AddToModStat;
                if (addToModStat != null && addToModStat.AffectedStat == StatType.HEALTH && addToModStat.Value < 0) {
                    healthDamage = addToModStat;
                }
            }
            if (healthDamage != null) {
                buffHolder.AddToStat(StatType.HEALTH, Stats.Set.MOD, healthDamage.Value);
            }
        }
    }

    public class ReflectAttack : Buff {
        private const int REFLECT_DAMAGE_RATIO = 1;

        public ReflectAttack() : base(Util.GetSprite("round-shield"), "Reflect Attack", "Next <color=yellow>Attack</color> on this unit is reflected.", false) {
        }

        /// <summary>
        /// The owner of the buff (the clone) is hit by an offensive spell that deals damage.
        /// </summary>
        public override bool IsReact(SingleSpell spellToReactTo, Stats owner) {
            return spellToReactTo.SpellBook.SpellType == SpellType.OFFENSE
                && spellToReactTo.Target.Stats == owner
                && spellToReactTo.Result.Type.IsSuccessfulType
                && spellToReactTo.Result.IsDealDamage;
        }

        /// <summary>
        ///  Add a self damage effect, reflecting the same amount as the attack.
        /// </summary>
        protected override void ReactHelper(SingleSpell spellToReactTo, Stats owner) {
            int damageToReflect = 0;
            foreach (SpellEffect se in spellToReactTo.Result.Effects) {
                AddToModStat damageHealth = se as AddToModStat;
                if (damageHealth != null && damageHealth.AffectedStat == StatType.HEALTH) {
                    damageToReflect = damageHealth.Value * REFLECT_DAMAGE_RATIO;
                }
            }
            spellToReactTo.Result.AddEffect(
                new AddToModStat(
                    spellToReactTo.Caster.Stats, StatType.HEALTH, damageToReflect)
                );
        }
    }

    public class Interceptor : Buff {
        private const int INTERCEPTION_DAMAGE = 2;

        // TODO add sprite
        public Interceptor() : base(Util.GetSprite("shark"), "Interceptor", "Unit will intercept attacks on its summoner.", false) { }

        public override bool IsReact(SingleSpell incomingSpell, Stats statsOfTheCharacterTheBuffIsOn) {
            bool isDealDamageToBuffCaster = false;

            // Intercepting adds an add to mod stat, so the spell technically does damage even after we wipe it
            // isDealDamage returns true regardless of whether the caster or target is taking damage
            // But we want to stop adding damage after the first tentacle intercepts the attack
            // which wipes the spell's effects and replaces it with a single addToModStat health damage
            foreach (SpellEffect se in incomingSpell.Result.Effects) {
                AddToModStat addToModStat = se as AddToModStat;
                if (addToModStat != null
                    && addToModStat.AffectedStat == StatType.HEALTH
                    && addToModStat.Value < 0
                    && addToModStat.Target == BuffCaster) {
                    isDealDamageToBuffCaster = true;
                }
            }

            return incomingSpell.Target.Stats == BuffCaster && isDealDamageToBuffCaster;
        }

        protected override void ReactHelper(SingleSpell s, Stats owner) {
            s.Result.Effects.Clear();
            s.Result.AddEffect(new AddToModStat(s.Caster.Stats, StatType.HEALTH, -INTERCEPTION_DAMAGE));
        }
    }

    public abstract class StatRegen : Buff {
        private int amountPerTurn;
        private StatType type;

        public StatRegen(StatType type, int amountPerTurn)
            : base(type.Sprite, string.Format("Restore {0}", type.Name), String.Format("Regenerate {0} {1} each turn.", amountPerTurn, type.ColoredName), false) {
            this.amountPerTurn = amountPerTurn;
            this.type = type;
        }

        protected override IList<SpellEffect> OnEndOfTurnHelper(Stats owner) {
            return new SpellEffect[] {
                new AddToModStat(owner, type, amountPerTurn)
            };
        }
    }

    public abstract class SirenSong : StatChange {
        private const int STAT_REDUCTION_PERCENT = 10;
        private const int DURATION = 3;

        public SirenSong(StatType type, string buffName)
            : base(DURATION, type, -STAT_REDUCTION_PERCENT, buffName, Util.GetSprite("g-clef")) { }
    }

    public abstract class StatChange : Buff {

        public StatChange(int duration, StatType type, int amount, string buffName, Sprite sprite)
            : base(
                  duration,
                  sprite,
                  buffName,
                  string.Format("{0} {1} by {2}%.", type.ColoredName, amount < 0 ? "decreased" : "increased", amount < 0 ? -amount : amount),
                  true) {
            Util.Assert(amount != 0, "Amount must be nonzero.");
            AddMultiplicativeStatBonus(type, amount);
        }

        public StatChange(int duration, StatType type, int amount)
            : this(
                      duration,
                      type,
                      amount,
                      string.Format("{0}{1}", type.Name, amount < 0 ? '-' : '+'),
                      type.Sprite) {
            Util.Assert(amount != 0, "Amount must be nonnegative.");
            AddMultiplicativeStatBonus(type, amount);
        }
    }
}