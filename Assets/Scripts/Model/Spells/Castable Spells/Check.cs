using Scripts.Model.Characters;
using Scripts.Model.Stats.Attributes;
using System;
using System.Collections.Generic;

namespace Scripts.Model.Spells.Named {

    public class Check : SpellFactory {

        public Check() : base("Check", "Check the attributes of a target.", SpellType.ACT, TargetType.ANY, null) {
        }

        public override Hit CreateHit() {
            return new Hit(
                isState: (c, t) => {
                    return true;
                },
                perform: (c, t, calc) => {
                    t.IsShowingBarCounts = true;
                },
                createText: (c, t, calc) => {
                    return CheckText(t);
                }
            );
        }

        private static string CheckText(Character target) {
            List<string> s = new List<string>();
            foreach (KeyValuePair<AttributeType, Stats.Attributes.Attribute> pair in target.Attributes) {
                if (pair.Key.IsAssignable) {
                    s.Add(
                        Util.Color(
                            string.Format(
                                "{0}+{1}/{2}",
                                target.Equipment.GetAttributeCount(pair.Key),
                                pair.Value.False,
                                pair.Value.True),
                                pair.Key.Color)
                                );
                }
            }
            return string.Format(
                "{0} Lv {1}\n{2}{3}{4}",
                target.DisplayName,
                target.Level,
                string.Join(" ", s.ToArray()),
                string.IsNullOrEmpty(target.CheckText) ? "" : string.Format("\n{0}", target.CheckText),
                target.Equipment.Count <= 0 ? "" : string.Format("\n{0}", target.Equipment.ToString())
                );
        }
    }
}