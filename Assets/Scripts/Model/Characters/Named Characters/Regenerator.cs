using UnityEngine;
using System.Collections;
using System;

public class Regenerator : ComputerCharacter {

    public Regenerator()
        : base("Icons/tentacle", "Regenerator", 1, 0, 1, 1, 2, Color.white, 2, "A tentacle with high life regeneration that can transfer its life to allies. Unable to attack.") {
    }

    public override void OnBattleStart() {
        (new Regenerate()).Cast(this, this);
    }

    protected override void DecideSpell() {
    }

    public override void OnDefeat(bool isSilent = false) {
        OnKill(isSilent);
    }
}
