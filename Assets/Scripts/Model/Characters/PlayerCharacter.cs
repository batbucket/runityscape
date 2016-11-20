using System;
using UnityEngine;

public abstract class PlayerCharacter : Character {
    public const float CHARGE_CAP_RATIO = 3f;

    public const bool DISPLAYABLE = true;

    // Debug Constructor
    public PlayerCharacter(string spriteLoc, string name, int level, int strength, int intelligence, int dexterity, int vitality, Color textColor)
        : base(new Inventory(), spriteLoc, name, level, strength, intelligence, dexterity, vitality, textColor, true) {
    }

    // Actual constructor
    public PlayerCharacter(Inventory inventory, string spriteLoc, string name, int level, int strength, int intelligence, int dexterity, int vitality, Color textColor)
    : base(inventory, spriteLoc, name, level, strength, intelligence, dexterity, vitality, textColor, true) {
    }

    public override void CalculateChargeRequirement(Character main) {
        CalculateChargeRequirement(this, main, CHARGE_CAP_RATIO);
    }

    public override void Act() {

    }

    protected override void WhileFullCharge() {

    }
}
