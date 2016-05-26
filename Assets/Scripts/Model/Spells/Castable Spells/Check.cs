using System;
using System.Collections.Generic;

public class Check : SpellFactory {
    public Check() : base("Check", "Check the attributes of a target.", SpellType.ACT, TargetType.ANY) { }

    protected override IDictionary<string, SpellComponent> CreateComponents(Character caster, Character target, Spell spell, SpellDetails other = null) {
        return new Dictionary<string, SpellComponent>() {
            { SpellFactory.PRIMARY, new SpellComponent(
                hit: new Result(
                    isState: (c, t, o) => {
                        return true;
                    },
                    createText: (c, t, calc, o) => {
                        List<string> s = new List<string>();
                        foreach (KeyValuePair<AttributeType, Attribute> pair in t.Attributes) {
                            s.Add(string.Format("{0} {1}/{2}", pair.Key.ShortName, pair.Value.False, pair.Value.True));
                        }
                        return string.Format("{0}  Lvl {1}\n{2}", t.Name, t.Level, string.Join("\n", s.ToArray()));
                    }
                    )
                ) }
        };
    }
}
