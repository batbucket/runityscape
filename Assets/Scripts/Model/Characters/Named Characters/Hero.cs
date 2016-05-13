using UnityEngine;
using System.Collections;

public class Hero : PlayerCharacter {
    public Hero(int strength, int intelligence, int dexterity, int vitality)
        : base(Util.GetSprite("placeholder"), "", 0, strength, intelligence, dexterity, vitality, Color.white) { }
}
