using UnityEngine;
using System.Collections;

public class Hero : PlayerCharacter {
    public Hero(string name, int strength, int intelligence, int dexterity, int vitality)
        : base(null, name, 0, strength, intelligence, dexterity, vitality, Color.white) { }
}
