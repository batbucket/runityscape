using Scripts.Game.Defined.Spells;
using Scripts.Model.Buffs;
using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using System.Collections.Generic;

namespace Scripts.Game.Defined.Serialized.Spells {
    public class Poison : Buff {
        public Poison() : base(2, Util.GetSprite("fox-head"), "Poisoned", "Loses health at the end of each turn.", true) { }

        protected override IList<SpellEffect> OnEndOfTurnHelper(Model.Characters.Stats owner) {
            return new SpellEffect[] {
                new AddToModStat(owner, StatType.HEALTH, -1)
            };
        }
    }

    public class Checked : Buff {
        public Checked() : base(5, Util.GetSprite("magnifying-glass"), "Checked", "Resource visibility increased.", true) { }

        protected override IList<SpellEffect> OnApplyHelper(Stats owner) { // TODO UNSTACKABLE
            return new SpellEffect[] { new AddToResourceVisibility(owner, 1) };
        }

        protected override IList<SpellEffect> OnTimeOutHelper(Stats owner) {
            return new SpellEffect[] { new AddToResourceVisibility(owner, -1) };
        }
    }

    public class Counter : Buff {
        public Counter() : base(Util.GetSprite("round-shield"), "Counter", "Next <color=yellow>Attack</color> on this unit is reflected.", false) { }

        public override bool IsReact(Spell s, Stats owner) {
            return s.Book is Attack && s.Target.Stats == owner;
        }

        protected override void ReactHelper(Spell s, Stats owner) {
            s.Result.Effects.Clear();
            s.Result.AddEffect(
                new AddToModStat(
                    s.Caster.Stats, StatType.HEALTH, -s.Caster.Stats.GetStatCount(Stats.Get.MOD, StatType.STRENGTH))
                );
        }
    }

    public class SpiritLink : Buff {
        public SpiritLink() : base(Util.GetSprite("knot"), "Spirit Link", "Attacks on the non-clone unit will also cause this unit to take damage.", false) {

        }

        public override bool IsReact(Spell s, Stats buffHolder) {
            return s.Book.SpellType == SpellType.OFFENSE && s.Target.Stats == this.BuffCaster && s.Result.Type.IsSuccessfulType;
        }

        protected override void ReactHelper(Spell s, Stats buffHolder) {
            SpellEffect healthDamage = null;
            for (int i = 0; (i < s.Result.Effects.Count) && (healthDamage == null); i++) {
                SpellEffect se = s.Result.Effects[i];
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
        public ReflectAttack() : base(Util.GetSprite("round-shield"), "Reflect Attack", "Next <color=yellow>Attack</color> on this unit is reflected.", false) { }

        public override bool IsReact(Spell s, Stats owner) {
            return s.Book is Attack && s.Target.Stats == owner && s.Result.Type.IsSuccessfulType;
        }

        protected override void ReactHelper(Spell s, Stats owner) {
            int damageToReflect = 0;
            foreach (SpellEffect se in s.Result.Effects) {
                AddToModStat damageHealth = se as AddToModStat;
                if (damageHealth != null && damageHealth.AffectedStat == StatType.HEALTH) {
                    damageToReflect = damageHealth.Value;
                }
            }
            s.Result.AddEffect(
                new AddToModStat(
                    s.Caster.Stats, StatType.HEALTH, damageToReflect)
                );
        }
    }
}