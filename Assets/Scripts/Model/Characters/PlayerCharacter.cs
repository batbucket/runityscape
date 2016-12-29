using System;
using UnityEngine;

public abstract class PlayerCharacter : Character {
    public const float CHARGE_CAP_RATIO = 3f;

    public const bool DISPLAYABLE = true;

    public override int ExperienceGiven {
        get {
            return 0;
        }
    }

    public override int GoldGiven {
        get {
            return 0;
        }
    }

    // Debug Constructor
    public PlayerCharacter(Displays displays, Attributes attributes)
        : base(DISPLAYABLE, new Inventory(), displays, attributes) {
    }

    // Actual Constructor
    public PlayerCharacter(Inventory inventory, Displays displays, Attributes attributes)
        : base(DISPLAYABLE, inventory, displays, attributes) {
    }

    public override void CalculateChargeRequirement(Character main) {
        CalculateChargeRequirement(this, main, CHARGE_CAP_RATIO);
    }

    protected override void Act() {

    }

    protected override void WhileFullCharge() {

    }
}
