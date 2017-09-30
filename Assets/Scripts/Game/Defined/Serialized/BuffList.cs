using Scripts.Game.Defined.Serialized.Spells;
using Scripts.Game.Defined.Spells;
using Scripts.Model.Buffs;
using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using System;
using System.Collections.Generic;

namespace Scripts.Game.Defined.Serialized.Buffs {

    public class Ignited : Buff {
        private const int DAMAGE_PER_TURN = 1;
        private const int DURATION = 5;

        public Ignited()
            : base(DURATION,
                  Util.GetSprite("fire"),
                  "Ignited",
                  string.Format("Take {0} damage at end of turn.",
                      DAMAGE_PER_TURN), true) {
        }

        protected override IList<SpellEffect> OnEndOfTurnHelper(Stats owner) {
            return new SpellEffect[] {
                new AddToModStat(owner, StatType.HEALTH, -DAMAGE_PER_TURN)
            };
        }
    }

    public class Insight : Buff {
        private const int MANA_RECOVERED_PER_TURN = 5;

        public Insight() : base(Util.GetSprite("water-drop"), "Insight", "Regenerating mana.", false) {
        }

        protected override IList<SpellEffect> OnEndOfTurnHelper(Stats owner) {
            return new SpellEffect[] {
                new AddToModStat(owner, StatType.MANA, MANA_RECOVERED_PER_TURN)
            };
        }
    }

    public class DamageResist : Buff {
        private const float DAMAGE_MULTIPLIER = 0.7f;

        public DamageResist()
            : base(Util.GetSprite("round-shield"),
                  "Damage Resist",
                  String.Format("Reduces incident damage by {0}%.",
                      (1 - DAMAGE_MULTIPLIER)), false) { }

        protected override void ReactHelper(Spell spellToReactTo, Stats owner) {
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

        public override bool IsReact(Spell spellToReactTo, Stats owner) {
            return spellToReactTo.Book is Attack && spellToReactTo.Target.Stats == owner;
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

        public override bool IsReact(Spell spellToReactTo, Stats owner) {
            return spellToReactTo.Book is Attack && spellToReactTo.Target.Stats == owner;
        }

        protected override void ReactHelper(Spell spellToReactTo, Stats owner) {
            spellToReactTo.Result.Effects.Clear();
            spellToReactTo.Result.AddEffect(
                new AddToModStat(
                    spellToReactTo.Caster.Stats, StatType.HEALTH,
                    -spellToReactTo.Caster.Stats.GetStatCount(Stats.Get.MOD, StatType.STRENGTH) * DAMAGE_RATIO_FROM_ATTACK)
                );
        }
    }

    public class SpiritLink : Buff {

        public SpiritLink() : base(Util.GetSprite("knot"), "Spirit Link", "Attacks on the non-clone unit will also cause this unit to take damage.", false) {
        }

        /// <summary>
        /// The caster of the buff (the non-clone) is hit by an offensive spell that deals damage.
        /// </summary>
        public override bool IsReact(Spell spellToReactTo, Stats buffHolder) {
            return spellToReactTo.Book.SpellType == SpellType.OFFENSE
                && spellToReactTo.Target.Stats == this.BuffCaster
                && spellToReactTo.Result.Type.IsSuccessfulType
                && spellToReactTo.Result.IsDealDamage;
        }

        protected override void ReactHelper(Spell spellToReactTo, Stats buffHolder) {
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
        public override bool IsReact(Spell spellToReactTo, Stats owner) {
            return spellToReactTo.Book.SpellType == SpellType.OFFENSE
                && spellToReactTo.Target.Stats == owner
                && spellToReactTo.Result.Type.IsSuccessfulType
                && spellToReactTo.Result.IsDealDamage;
        }

        /// <summary>
        ///  Add a self damage effect, reflecting the same amount as the attack.
        /// </summary>
        protected override void ReactHelper(Spell spellToReactTo, Stats owner) {
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

    public class Restore : Buff {
        private const int REGEN_PER_TURN = 1;

        public Restore() : base(Util.GetSprite("health-normal"), "Restore", String.Format("Restores {0} health each turn.", REGEN_PER_TURN), false) {
        }

        protected override IList<SpellEffect> OnEndOfTurnHelper(Model.Characters.Stats owner) {
            return new SpellEffect[] {
                new AddToModStat(owner, StatType.HEALTH, REGEN_PER_TURN)
            };
        }
    }
}