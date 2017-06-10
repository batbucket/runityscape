using System;
using System.Collections.Generic;
using Scripts.Model.Characters;
using Scripts.Model.Stats;
using UnityEngine;
using System.Text;
using Scripts.Model.Buffs;

namespace Scripts.Model.Spells {
    public static class SpellEffectList {
        public class AddToModStat : SpellEffect {
            private readonly StatType affected;
            private readonly Characters.Stats target;

            public AddToModStat(Characters.Stats target, StatType affected, int value) : base(SpellEffectType.ADD_TO_MOD_STAT, value) {
                this.affected = affected;
                this.target = target;
            }

            public override void CauseEffect() {
                target.AddToStat(affected, Stats.Value.MOD, Value);
            }
        }

        public class AddBuff : SpellEffect {
            private readonly Buff buff;
            private readonly Characters.Buffs target;

            public AddBuff(Characters.Buffs target, Buff buff) : base(SpellEffectType.ADD_BUFF, 1) {
                this.target = target;
                this.buff = buff;
            }

            public override void CauseEffect() {
                target.AddBuff(buff);
            }
        }
    }
}