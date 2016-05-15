using UnityEngine;
using System.Collections;
using System;

public class Guardian : ComputerCharacter {
    public Guardian() : base(Util.GetSprite("placeholder"), "Unknown", 999, 999, 999, 999, 999, Color.yellow, 0) { }

    protected override void DecideSpell() {
        throw new NotImplementedException();
    }

    protected override void WhileFullCharge() {
        throw new NotImplementedException();
    }
}
