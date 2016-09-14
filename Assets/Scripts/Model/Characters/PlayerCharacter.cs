using System;
using UnityEngine;

public abstract class PlayerCharacter : Character {
    public const float CHARGE_CAP_RATIO = 3f;

    public const bool DISPLAYABLE = true;

    public PlayerCharacter(string spriteLoc, string name, int level, int strength, int intelligence, int dexterity, int vitality, Color textColor)
        : base(spriteLoc, name, level, strength, intelligence, dexterity, vitality, textColor, true) {
    }

    public override void CalculateChargeRequirement(Character main) {
        CalculateChargeRequirement(this, main, CHARGE_CAP_RATIO);
    }

    public override void Act() {

    }

    protected override void WhileFullCharge() {

    }
}
