using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Stats;
using Scripts.View.Portraits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Model.SaveLoad;
using System.Collections.ObjectModel;
using System.Linq;
using Scripts.Model.Pages;
using Scripts.Game.Defined.SFXs;
using Scripts.Presenter;
using Scripts.Model.Tooltips;
using System;

namespace Scripts.Model.Spells {

    public struct SpellParams {
        public readonly int CharacterId;
        public readonly Character Character;
        public readonly Characters.Look Look;
        public readonly Characters.Stats Stats;
        public readonly Characters.SpellBooks Spells;
        public readonly Characters.Buffs Buffs;
        public readonly Characters.Inventory Inventory;
        public readonly Characters.Equipment Equipment;
        public readonly Page Page;
        public PortraitView Portrait {
            get {
                return Character.Presenter.PortraitView;
            }
        }

        public SpellParams(Characters.Character c, Page currentPage) {
            this.CharacterId = c.Id;
            this.Character = c;
            this.Look = c.Look;
            this.Stats = c.Stats;
            this.Spells = c.Spells;
            this.Buffs = c.Buffs;
            this.Inventory = c.Inventory;
            this.Equipment = c.Equipment;
            this.Page = currentPage;
        }
    }

    public abstract class SpellBook : ISpellable, ISaveable<SpellBookSave> {

        public readonly string Name;
        public readonly Sprite Icon;
        public readonly TargetType TargetType;
        public readonly SpellType SpellType;
        public readonly PriorityType Priority;

        public readonly int CastTime;
        public readonly int Cooldown;
        public readonly bool IsSilenced;
        public readonly string Verb;

        protected readonly HashSet<Flag> flags;
        private readonly IDictionary<StatType, int> costs;

        public SpellBook(string spellName, Sprite sprite, TargetType target, SpellType spell, int castTime, int cooldown, string verb) : this(spellName, sprite, target, spell, castTime, cooldown, 0, verb) { }

        public SpellBook(string spellName, Sprite sprite, TargetType target, SpellType spell, int castTime, int cooldown, PriorityType priority, string verb) {
            this.Name = spellName;
            this.Icon = sprite;
            this.costs = new Dictionary<StatType, int>();
            this.TargetType = target;
            this.SpellType = spell;
            this.CastTime = castTime;
            this.Cooldown = cooldown;
            this.flags = new HashSet<Flag>() { Flag.CASTER_REQUIRES_SPELL };
            this.Verb = verb;
            this.Priority = priority;
        }

        public SpellBook(string spellName, string spriteLoc, TargetType target, SpellType spell, int castTime, int cooldown)
            : this(spellName, Util.GetSprite(spriteLoc), target, spell, castTime, cooldown, "use") { }

        public override bool Equals(object obj) {
            var item = obj as SpellBook;

            if (item == null) {
                return false;
            }

            return this.GetType().Equals(item.GetType())
                && this.IsSilenced.Equals(item.IsSilenced);
        }

        public IDictionary<StatType, int> Costs {
            get {
                return new ReadOnlyDictionary<StatType, int>(costs);
            }
        }

        public virtual TooltipBundle TextboxTooltip {
            get {
                return new TooltipBundle(
                    this.Icon,
                    this.Name,
                    string.Format("{0}{1}{2}",
                        Priority == 0 ? string.Empty : string.Format("{0} priority\n", Priority.GetDescription()),
                        Costs.Count == 0 ? string.Empty : string.Format("Costs {0}\n\n", GetCommaSeparatedCosts()),
                        CreateDescriptionHelper()
                        )
                    );
            }
        }

        public override int GetHashCode() {
            return GetType().GetHashCode();
        }

        public bool HasFlag(Flag f) {
            return flags.Contains(f);
        }

        public string CreateDescription(SpellParams caster) {
            return string.Format("{0}{1}{2}{3}",
                CasterHasResources(caster.Stats) ? string.Empty : Util.ColorString("Insufficient resource.\n", Color.red),
                Priority == 0 ? string.Empty : string.Format("{0} priority", Priority.GetDescription()),
                Costs.Count == 0 ? string.Empty : string.Format("\nCosts {0}\n\n", GetCommaSeparatedCosts(caster.Stats)),
                CreateDescriptionHelper()
                );
        }

        public string CreateTargetDescription(SpellParams caster, SpellParams target) {
            return string.Format("{0} {1} on {2}.", this.Verb, this.Name, target.Look.DisplayName);
        }

        protected abstract string CreateDescriptionHelper();

        public bool IsMeetPreTargetRequirements(Characters.Stats caster) {
            return CasterHasResources(caster) && IsMeetOtherPreTargetRequirements();
        }

        protected virtual bool IsMeetOtherPreTargetRequirements() {
            return true;
        }

        public bool CasterHasResource(StatType stat, Characters.Stats caster) {
            return caster.GetStatCount(Characters.Stats.Get.MOD, stat) >= Costs[stat];
        }

        public bool CasterHasResources(Characters.Stats caster) {
            foreach (KeyValuePair<StatType, int> stat in Costs) {
                if (caster.GetStatCount(Characters.Stats.Get.MOD, stat.Key) < stat.Value) {
                    return false;
                }
            }
            return true;
        }

        public bool IsCastable(SpellParams caster, SpellParams target) {
            return !IsSilenced && CasterHasResources(caster.Stats) && IsMeetOtherCastRequirements(caster, target) && (caster.Spells.HasSpellBook(this) || !flags.Contains(Flag.CASTER_REQUIRES_SPELL));
        }

        public Spell BuildSpell(SpellParams caster, SpellParams target) {
            Util.Assert(
                IsCastable(caster, target),
                string.Format(
                "Attempted to cast spell without requirements fulfilled. Resources={0}, OtherRequirements={1}."
                , CasterHasResources(caster.Stats),
                IsMeetOtherCastRequirements(caster, target)
                ));

            // Consume resources
            foreach (KeyValuePair<StatType, int> pair in Costs) {
                caster.Stats.AddToStat(pair.Key, Characters.Stats.Set.MOD, -pair.Value);
            }

            return ForceSpell(caster, target);
        }

        public Spell ForceSpell(SpellParams caster, SpellParams target) {
            Result res = new Result();
            if (IsHit(caster, target)) {
                res.AddSFX(GetHitSFX(caster.Portrait, target.Portrait));
                if (IsCritical(caster, target)) {
                    res.Type = ResultType.CRITICAL;
                    res.AddEffects(GetCriticalEffects(caster, target));
                } else {
                    res.Type = ResultType.HIT;
                    res.AddEffects(GetHitEffects(caster, target));
                }
            } else {
                res.AddSFX(GetMissSFX(caster.Portrait, target.Portrait));
                res.AddSFX(SFX.HitSplat(target.Portrait.EffectsHolder, new SplatDetails(Color.grey, "MISS!")));
                res.Type = ResultType.MISS;
                res.AddEffects(GetMissEffects(caster, target));
            }

            return new Spell(this, res, caster, target);
        }

        public override string ToString() {
            return base.ToString();
        }

        protected virtual bool IsMeetOtherCastRequirements(SpellParams caster, SpellParams target) {
            return true;
        }

        protected virtual bool IsHit(SpellParams caster, SpellParams target) {
            return true;
        }

        protected abstract IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target);

        protected virtual bool IsCritical(SpellParams caster, SpellParams target) {
            return false;
        }

        protected virtual IList<SpellEffect> GetCriticalEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[0];
        }

        protected virtual IList<SpellEffect> GetMissEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[0];
        }

        protected virtual IList<IEnumerator> GetHitSFX(PortraitView caster, PortraitView target) {
            return new IEnumerator[0];
        }

        protected void AddCost(StatType type, int cost) {
            Util.Assert(cost > 0, "Cost must be positive.");
            costs.Add(type, cost);
        }

        protected virtual IList<IEnumerator> GetMissSFX(PortraitView caster, PortraitView target) {
            return new IEnumerator[0];
        }

        private string GetCommaSeparatedCosts(Characters.Stats caster = null) {
            string[] arr = new string[Costs.Count];

            int index = 0;
            foreach (KeyValuePair<StatType, int> pair in Costs) {
                arr[index++] = string.Format("{0} {1}",
                    Util.ColorString(pair.Value.ToString(), caster != null && CasterHasResource(pair.Key, caster)),
                    Util.ColorString(pair.Key.Name, pair.Key.Color));
            }
            return string.Join(", ", arr);
        }

        SpellBook ISpellable.GetSpellBook() {
            return this;
        }

        public virtual string GetDetailedName(SpellParams caster) {
            return Util.ColorString(string.Format("{0}", Name), CasterHasResources(caster.Stats));
        }

        public SpellBookSave GetSaveObject() {
            return new SpellBookSave(GetType());
        }

        public void InitFromSaveObject(SpellBookSave saveObject) {
            // Spellbook doesn't need anything restored!
        }
    }
}