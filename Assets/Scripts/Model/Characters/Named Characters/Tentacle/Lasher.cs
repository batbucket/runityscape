using UnityEngine;
using System.Collections;
using System;

public class Lasher : Mimic {
    public Lasher()
        : base(
            new Displays {
                Loc = "Icons/spiked-tentacle",
                Name = "Lasher",
                Color = Color.white,
                Check = "A tentacle optimized for combat."
            },
            new Displays {
                Loc = "Icons/gladius",
                Name = "Knight?",
                Color = Color.white,
                Check = "A knight in shining white armor, sans horse. Rumored to have an eccentric and suboptimal walking style."
            },
            new Attributes {
                Lvl = 1,
                Str = 1,
                Int = 1,
                Agi = 1,
                Vit = 1
            }
            ) {

    }

    protected override void DecideSpell() {
        if (isTransformed) {
            QuickCast(new Lash());
        } else {
            QuickCast(new Attack());
        }
    }

    public override Mimic Summon() {
        return new Lasher();
    }
}
