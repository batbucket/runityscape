using UnityEngine;
using System.Collections;
using System;

public class Alestre : ComputerCharacter {
    public Alestre() : base("placeholder", "Alestre", 0, 100, 100, 100, 100, Color.yellow, 0, "") {

    }

    protected override void DecideSpell() {
        throw new NotImplementedException();
    }
}
