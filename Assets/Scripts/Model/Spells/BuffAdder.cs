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
    public abstract class BuffAdder : BasicSpellbook {
        private Buff dummy;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuffAdder"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="spell">The spell.</param>
        /// <param name="dummy">The dummy.</param>
        /// <param name="name">The name.</param>
        /// <param name="priority">The priority.</param>
        public BuffAdder(TargetType target, SpellType spell, Buff dummy, string name, PriorityType priority) : this(target, spell, dummy, dummy.Sprite) { }

        /// <summary>
        /// Main
        /// </summary>
        /// <param name="target">Type of targets</param>
        /// <param name="spell">Type of spell</param>
        /// <param name="dummy">Dummy buff to create text and copies</param>
        /// <param name="name">Spell name</param>
        /// <param name="priority">Priority of the spell</param>
        public BuffAdder(TargetType target, SpellType spell, Buff dummy, string name, PriorityType priority, Sprite sprite) : base(name, sprite, target, spell, priority) {
            this.dummy = dummy;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuffAdder"/> class.
        /// This one makes the name of the Buff the Spell as well.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="spell">The spell.</param>
        /// <param name="dummy">The dummy.</param>
        public BuffAdder(TargetType target, SpellType spell, Buff dummy, Sprite sprite) : this(target, spell, dummy, dummy.Name, PriorityType.NORMAL, sprite) { }

        /// <summary>
        /// Does the buff adding
        /// </summary>
        /// <returns>IList with neccessar spell effects for adding a buff</returns>
        protected sealed override IList<SpellEffect> GetHitEffects(Page page, Character caster, Character target) {
            return new SpellEffect[] {
                new AddBuff(
                    new BuffParams(caster.Stats, caster.Id),
                    target.Buffs,
                    Util.TypeToObject<Buff>(dummy.GetType()))
            };
        }

        protected sealed override string CreateDescriptionHelper() {
            return CreateBuffDescription(this.TargetType, dummy);
        }

        /// <summary>
        /// Used to create a descriptive buff tooltip.
        /// </summary>
        /// <param name="buff">Buff to create a description for.</param>
        /// <returns>Descriptive buff tooltip</returns>
        public static string CreateBuffDescription(TargetType target, Buff buff) {
            return string.Format(
                "{0} is affected by <color=cyan>{1}</color>.\n{2}\nLasts {3} turns.",
                target.Name,
                buff.Name,
                buff.Description,
                buff.DurationText
                );
        }
    }
}