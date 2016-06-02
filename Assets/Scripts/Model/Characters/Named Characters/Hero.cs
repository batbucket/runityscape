using UnityEngine;
using System.Collections;

public class Hero : PlayerCharacter {
    public Hero(int strength, int intelligence, int dexterity, int vitality)
        : base("placeholder", "Brandon", 0, strength, intelligence, dexterity, vitality, Color.white) {
        this.Attack = new Attack();
        this.Selections[Selection.ACT].Add(new Check());
        this.AddResource(new NamedResource.Skill());
    }
}
