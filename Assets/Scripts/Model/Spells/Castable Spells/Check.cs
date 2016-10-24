using System;
using System.Collections.Generic;

public class Check : SpellFactory {
    public Check() : base("Check", "Check the attributes of a target.", SpellType.ACT, TargetType.ANY) { }

    public override Hit CreateHit() {
        return new Hit(
            isState: (c, t, o) => {
                return true;
            },
            perform: (c, t, calc, o) => {
                t.IsShowingBarCounts = true;
            },
            createText: (c, t, calc, o) => {
                return CheckText(t);
            }
        );
    }

    private static string CheckText(Character target) {
        List<string> s = new List<string>();
        foreach (KeyValuePair<AttributeType, Attribute> pair in target.Attributes) {
            if (pair.Key.IsAssignable) {
                s.Add(Util.Color(string.Format("{0}/{1}", pair.Value.False, pair.Value.True), pair.Key.Color));
            }
        }
        return string.Format(
            "{0} Lv {1}\n{2}{3}{4}",
            target.DisplayName,
            target.Level.False,
            string.Join(" ", s.ToArray()),
            string.IsNullOrEmpty(target.CheckText) ? "" : string.Format("\n{0}", target.CheckText),
            target.Equipment.Count <= 0 ? "" : string.Format("\n{0}", target.Equipment.ToString())
            );
    }
}
