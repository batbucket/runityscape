using UnityEngine;
using System.Collections;
using System;

public class Regenerate : SpellFactory {
    public Regenerate() : base("Regenerate", "", SpellType.OFFENSE, TargetType.SINGLE_ENEMY, abbreviation: "REG", color: Color.green, isSelfTargetable: true) { }

    public override Hit CreateHit() {
        return new Hit(
            isState: (c, t, o) => true,
            isIndefinite: (c, t, o) => true,
            timePerTick: (c, t, o) => 0f,
            perform: (c, t, calc, o) => t.AddToResource(ResourceType.HEALTH, false, 0.02f),
            sfx: (c, t, calc, o) => new CharacterEffect[0]
            );
    }
}
