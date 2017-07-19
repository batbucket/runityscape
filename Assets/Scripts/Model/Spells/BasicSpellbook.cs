using UnityEngine;

namespace Scripts.Model.Spells {

    /// <summary>
    /// Most spellbooks can't target dead people and don't have cooldowns.
    /// </summary>
    public abstract class BasicSpellbook : SpellBook {
        public BasicSpellbook(string spellName, Sprite sprite, TargetType target, SpellType spell) : base(spellName, sprite, target, spell, 0, 0, "Perform") { }

        public BasicSpellbook(string spellName, Sprite sprite, TargetType target, SpellType spell, int priority) : base(spellName, sprite, target, spell, 0, 0, priority, "Perform") { }

        protected sealed override bool IsMeetOtherCastRequirements(SpellParams caster, SpellParams target) {
            return caster.Stats.State == Characters.State.ALIVE && target.Stats.State == Characters.State.ALIVE && IsMeetCastRequirements(caster, target);
        }

        protected virtual bool IsMeetCastRequirements(SpellParams caster, SpellParams target) {
            return true;
        }
    }

}