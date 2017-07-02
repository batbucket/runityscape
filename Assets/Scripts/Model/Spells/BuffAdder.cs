using Scripts.Game.Defined.Spells;
using Scripts.Model.Buffs;
using System;
using System.Collections.Generic;

namespace Scripts.Model.Spells {
    public abstract class BuffAdder : BasicSpellbook {
        private Buff dummy;

        public BuffAdder(TargetType target, SpellType spell, Buff dummy, string name) : base(name, dummy.Sprite, target, spell) {
            this.dummy = dummy;
        }

        protected sealed override IList<SpellEffect> GetHitEffects(SpellParams caster, SpellParams target) {
            return new SpellEffect[] {
                new AddBuff(
                    new BuffParams(caster.Stats, caster.CharacterId),
                    target.Buffs,
                    Util.TypeToObject<Buff>(dummy.GetType()))
            };
        }

        public sealed override string CreateDescriptionHelper(SpellParams caster) {
            return CreateBuffDescription(dummy);
        }

        public static string CreateBuffDescription(Buff buff) {
            return string.Format(
                "Target is affected by\n\n<color=cyan>{0}</color>\n{1}\nDuration: {2} turns.",
                buff.Name,
                buff.Description,
                buff.DurationText
                );
        }

    }
}