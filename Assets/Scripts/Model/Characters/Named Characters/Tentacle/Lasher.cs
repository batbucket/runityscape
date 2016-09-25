using UnityEngine;
using System.Collections;
using System;

public class Lasher : Tentacle {
    public Lasher() : base("Icons/spiked-tentacle", "Lasher", 1, 3, 0, 1, 2, Color.white, 3f, "A tentacle adapted for combat.") {

    }

    protected override void DecideSpell() {
        QuickCast(new Lash());
    }
}
