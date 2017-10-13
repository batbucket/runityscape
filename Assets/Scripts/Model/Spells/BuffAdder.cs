using Scripts.Game.Defined.Spells;
using Scripts.Model.Buffs;
using Scripts.Model.Characters;
using Scripts.Model.Pages;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Spells {

    /// <summary>
    /// Convenience spellbook extension for creating SpellBooks
    /// that only add a buff.
    /// </summary>
    /// <seealso cref="Scripts.Model.Spells.BasicSpellbook" />
    public abstract class BuffAdder<T> : BasicSpellbook where T : Buff {
        protected bool isBuffUnique;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuffAdder"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="spell">The spell.</param>
        /// <param name="name">The name.</param>
        /// <param name="priority">The priority.</param>
        public BuffAdder(TargetType target, SpellType spell, string name, PriorityType priority) : this(target, spell, name, priority, Buff.Sprite) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuffAdder"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="spell">The spell.</param>
        /// <param name="name">The name.</param>
        /// <param name="priority">The priority.</param>
        public BuffAdder(TargetType target, SpellType spell, PriorityType priority) : this(target, spell, Buff.Name, priority, Buff.Sprite) { }

        /// <summary>
        /// Main
        /// </summary>
        /// <param name="target">Type of targets</param>
        /// <param name="spell">Type of spell</param>
        /// <param name="name">Spell name</param>
        /// <param name="priority">Priority of the spell</param>
        public BuffAdder(TargetType target, SpellType spell, string name, PriorityType priority, Sprite sprite) : base(name, sprite, target, spell, priority) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuffAdder"/> class.
        /// This one makes the name of the Buff the Spell as well.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="spell">The spell.</param>
        public BuffAdder(TargetType target, SpellType spell, Sprite sprite) : this(target, spell, Buff.Name, PriorityType.NORMAL, Buff.Sprite) { }

        private static Buff dummy;

        private static Buff Buff {
            get {
                if (dummy == null) {
                    dummy = Util.TypeToObject<T>(typeof(T));
                }
                return dummy;
            }
        }

        /// <summary>
        /// Does the buff adding
        /// </summary>
        /// <returns>IList with neccessar spell effects for adding a buff</returns>
        protected sealed override IList<SpellEffect> GetHitEffects(Page page, Character caster, Character target) {
            List<SpellEffect> effects = new List<SpellEffect>();

            effects.Add(new AddBuff(
                    new BuffParams(caster.Stats, caster.Id),
                    target.Buffs,
                    Util.TypeToObject<Buff>(Buff.GetType())));

            return effects.ToArray();
        }

        protected sealed override string CreateDescriptionHelper() {
            return CreateBuffDescription(this.TargetType, Buff);
        }

        protected override bool IsMeetCastRequirements(Character caster, Character target) {
            return !isBuffUnique || !target.Buffs.HasBuff<T>();
        }

        /// <summary>
        /// Used to create a descriptive buff tooltip.
        /// </summary>
        /// <param name="buff">Buff to create a description for.</param>
        /// <returns>Descriptive buff tooltip</returns>
        public static string CreateBuffDescription(TargetType target, Buff buff) {
            return string.Format(
                "{0} affected by\n{1}\nLasts {2} turns.",
                target.Name,
                buff.StringTooltip,
                buff.DurationText
                );
        }
    }
}