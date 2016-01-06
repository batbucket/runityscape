using UnityEngine;
using System.Collections;
using System;

public abstract class ComputerCharacter : Character {

    public ComputerCharacter(Sprite sprite, string name, int level, int strength, int intelligence, int dexterity, int vitality)
        : base(sprite, name, level, strength, intelligence, dexterity, vitality) {
    }

    public override void act(int chargeAmount, Game game) {
        charge(chargeAmount);
    }
}
