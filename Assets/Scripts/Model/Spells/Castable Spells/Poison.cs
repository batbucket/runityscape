using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Poison : SpellFactory {
    public Poison()
        : base("Poison", "Poison an enemy.", SpellType.OFFENSE, TargetType.SINGLE_ENEMY, abbreviation: "PSN", color: Color.magenta) { }

    public override Hit CreateHit() {
        return new Hit(
            isState:
            (c, t, o) => true,
            duration:
            (c, t, o) => {
                return 30;
            },
            timePerTick:
            (c, t, o) => {
                return 1.5f;
            },
            calculation:
            (c, t, o) => {
                return new Calculation(targetResources: new Dictionary<ResourceType, PairedInt>() { { ResourceType.HEALTH, new PairedInt(0, -2) } });
            },
            createText:
            (c, t, calc, o) => {
                return string.Format("{0} is <color=magenta>poisoned</color>!\n{0} took {1} damage!", t.DisplayName, Util.Color("" + -calc.TargetResources[ResourceType.HEALTH].False, Color.red));
            },
            sound:
            (c, t, calc, o) => {
                return "Blip_1";
            }
            );
    }
}
