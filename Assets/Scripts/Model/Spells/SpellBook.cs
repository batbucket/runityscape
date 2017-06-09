using Scripts.Model.Characters;
using Scripts.Model.Stats;
using Scripts.View.Portraits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Spells {

    public abstract class SpellBook {
        public readonly string Name;
        public readonly Sprite Icon;
        public readonly TargetType TargetType;
        public readonly SpellType SpellType;
        public readonly IDictionary<StatType, int> Costs;
        public readonly int Priority;

        public int CastTime;
        public int Cooldown;
        public bool IsSilenced;

        public SpellBook(string spellName, string spriteLoc, TargetType target, SpellType spell, int castTime, int cooldown) {
            this.Name = spellName;
            this.Icon = Util.LoadIcon(spriteLoc);
            this.Costs = new Dictionary<StatType, int>();
            this.TargetType = target;
            this.SpellType = spell;
            this.CastTime = castTime;
            this.Cooldown = cooldown;
        }

        public override bool Equals(object obj) {
            SpellBook item = obj as SpellBook;

            if (item == null) {
                return false;
            }

            return this.Name == item.Name;
        }

        public override int GetHashCode() {
            return Name.GetHashCode();
        }

        public string CreateDescription(Characters.Stats caster) {
            // TODO target type and etc.
            return CreateDescriptionHelper(caster);
        }

        public abstract string CreateDescriptionHelper(Characters.Stats caster);

        public bool CasterHasResources(Characters.Stats caster) {
            foreach (KeyValuePair<StatType, int> stat in Costs) {
                if (caster.GetStatCount(Value.MOD, stat.Key) < stat.Value) {
                    return false;
                }
            }
            return true;
        }

        public bool IsCastable(Characters.Spells casterSpells, Characters.Stats caster, Characters.Stats target) {
            return CasterHasResources(caster) && IsMeetOtherCastRequirements(caster, target) && casterSpells.HasSpellBook(this);
        }

        public Spell BuildSpell(Character caster, Character target) {
            Util.Assert(
                (IsCastable(caster.Spells, caster.Stats, target.Stats)),
                string.Format(
                "Attempted to cast spell without requirements fulfilled. Resources={0}, OtherRequirements={1}."
                , CasterHasResources(caster.Stats),
                IsMeetOtherCastRequirements(caster.Stats, target.Stats)
                ));

            // Consume resources
            foreach (KeyValuePair<StatType, int> pair in Costs) {
                caster.Stats.AddToStat(pair.Key, -pair.Value);
            }

            return ForceSpell(caster, target);
        }

        public Spell ForceSpell(Character caster, Character target) {
            Characters.Stats casterStats = caster.Stats;
            Characters.Stats targetStats = target.Stats;

            Result res = new Result();
            if (IsHit(casterStats, targetStats)) {
                res.AddSFX(GetHitSFX(caster.Presenter.PortraitView, target.Presenter.PortraitView));
                if (IsCritical(casterStats, targetStats)) {
                    res.Type = ResultType.CRITICAL;
                    res.Effects = GetCriticalEffects(casterStats, targetStats);
                } else {
                    res.Type = ResultType.HIT;
                    res.Effects = GetHitEffects(casterStats, targetStats);
                }
            } else {
                res.AddSFX(GetMissSFX(caster.Presenter.PortraitView, target.Presenter.PortraitView));
                res.Type = ResultType.MISS;
                res.Effects = GetMissEffects(casterStats, targetStats);
            }

            return new Spell(this, res, caster, target);
        }

        protected virtual bool IsMeetOtherCastRequirements(Characters.Stats caster, Characters.Stats target) {
            return true;
        }

        protected virtual bool IsHit(Characters.Stats caster, Characters.Stats target) {
            return true;
        }

        protected abstract IList<SpellEffect> GetHitEffects(Characters.Stats caster, Characters.Stats target);

        protected virtual bool IsCritical(Characters.Stats caster, Characters.Stats target) {
            return false;
        }

        protected virtual IList<SpellEffect> GetCriticalEffects(Characters.Stats caster, Characters.Stats target) {
            return new SpellEffect[0];
        }

        protected virtual IList<SpellEffect> GetMissEffects(Characters.Stats caster, Characters.Stats target) {
            return new SpellEffect[0];
        }

        protected virtual IList<IEnumerator> GetHitSFX(PortraitView caster, PortraitView target) {
            return new IEnumerator[0];
        }

        protected virtual IList<IEnumerator> GetMissSFX(PortraitView caster, PortraitView target) {
            return new IEnumerator[0];
        }
    }
}