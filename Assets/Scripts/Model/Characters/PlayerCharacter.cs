using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class PlayerCharacter : Character {
    public const bool DISPLAYABLE = true;

    public PlayerCharacter(Sprite sprite, string name, int level, int strength, int intelligence, int dexterity, int vitality, Color textColor)
        : base(sprite, name, level, strength, intelligence, dexterity, vitality, textColor, true) {
    }

    public override void Act(int chargeAmount, Game game) {
        base.Act(chargeAmount, game);
    }

    public override void OnStart(Game game) {
    }
}
