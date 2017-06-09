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

            public AddToModStat(Characters.Stats target, StatType affected, int value) : base(SpellEffectType.ADD_TO_MOD_STAT, target, value) {
                this.affected = affected;
            }

            public override SplatDetails SplatDetails {
                get {
                    Color color = Color.grey;
                    if (Value > 0) {
                        color = affected.Color;
                    } else if (Value < 0) {
                        color = affected.NegativeColor;
                    } else {
                        color = Color.gray;
                    }

                    return new SplatDetails(color, Value.ToString("+#;-#;0"), affected.Sprite);
                }
            }

            public override void CauseEffect() {
                Target.AddToStat(affected, Value);
            }
        }

        public class AddBuff : SpellEffect {
            private readonly Buff buff;

            public AddBuff(Characters.Stats target, Buff buff) : base(SpellEffectType.ADD_BUFF, target, buff.TurnsRemaining) {
                this.buff = buff;
            }

            public override SplatDetails SplatDetails {
                get {
                    return new SplatDetails(Color.white, string.Format("+{0}", buff.Name));
                }
            }

            public override void CauseEffect() {
                Target.Buffs.AddBuff(buff);
            }
        }
    }
}