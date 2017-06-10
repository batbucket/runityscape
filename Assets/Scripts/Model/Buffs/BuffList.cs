using Scripts.Model.Buffs;
using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using System.Collections.Generic;

public static class BuffList {
    public class Poison : Buff {
        public Poison(SpellParams caster, SpellParams target) : base(caster, target, Util.GetSprite("fox-head"), "Poisoned", "Loses health at the end of each turn.") { }

        protected override IList<SpellEffect> OnEndOfTurnHelper() {
            return new SpellEffect[] {
                new SpellEffectList.AddToModStat(Target.Stats, StatType.HEALTH, -1)
            };
        }
    }
}