using System;
using UnityEngine;

public abstract class PlayerCharacter : Character {

    public const bool DISPLAYABLE = true;

    public PlayerCharacter(Displays displays, Attributes attributes, Items items)
        : base(DISPLAYABLE, displays, attributes, items) {
    }

    public PlayerCharacter(Displays displays, Attributes attributes)
        : base(DISPLAYABLE, displays, attributes) {
    }

    protected override void Act() {

    }

    protected override void WhileFullCharge() {

    }
}
