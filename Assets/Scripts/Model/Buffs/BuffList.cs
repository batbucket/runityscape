using Scripts.Model.Buffs;
using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using System.Collections.Generic;

public static class BuffList {
    public class Poison : Buff {
        public Poison(Stats caster, Stats target) : base(caster, target, Util.GetSprite("fox-head"), "Poisoned", "Loses health at the end of each turn.") { }

        public override IList<SpellEffect> OnEndOfTurn() {
            return new SpellEffect[] {
                new SpellEffectList.AddToModStat(Target, StatType.HEALTH, -1)
            };
        }
    }
}