using UnityEngine;
using System.Collections;
using System;

public class Regenerator : ComputerCharacter {

    public Regenerator()
        : base("Icons/tentacle", "Regenerator", 1, 1, 1, 1, 2, Color.white, 2, "A tentacle with high life regeneration. Can transfer its life to allies.") {
    }

    public override void OnBattleStart() {
        (new Regenerate()).Cast(this, this);
    }

    protected override void DecideSpell() {
        QuickCast(new Attack());
    }
}
