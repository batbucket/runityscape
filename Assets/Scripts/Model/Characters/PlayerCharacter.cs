using UnityEngine;

public abstract class PlayerCharacter : Character {
    public const bool DISPLAYABLE = true;

    public PlayerCharacter(string spriteLoc, string name, int level, int strength, int intelligence, int dexterity, int vitality, Color textColor)
        : base(spriteLoc, name, level, strength, intelligence, dexterity, vitality, textColor, true) {
    }

    public override void Act() {

    }

    protected override void WhileFullCharge() {

    }
}
