using UnityEngine;
using System.Collections;
using System;

public class Regenerate : SpellFactory {
    public Regenerate() : base("Regenerate", "Recover health over time.", SpellType.DEFENSE, TargetType.SELF, abbreviation: "REG", color: Color.green, isSelfTargetable: true) { }

    public override Hit CreateHit() {
        return new Hit(
            isState: (c, t, o) => true,
            isIndefinite: (c, t, o) => true,
            timePerTick: (c, t, o) => 0f,
            perform: (c, t, calc, o) => t.AddToResource(ResourceType.HEALTH, false, 0.05f),
            createText: (c, t, calc, o) => string.Format("{0} is regenerating life.", c.DisplayName),
            sfx: (c, t, calc, o) => new CharacterEffect[0]
            );
    }
}
