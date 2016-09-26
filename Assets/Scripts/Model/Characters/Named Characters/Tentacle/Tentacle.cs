using UnityEngine;
using System.Collections;

/// <summary>
/// Don't have a mortal damage state. Can be summoned.
/// </summary>
public abstract class Tentacle : ComputerCharacter {

    public Tentacle(string spriteLoc, string name, int level, int strength, int intelligence, int dexterity, int vitality, Color textColor, float maxDelay, string checkText)
        : base(spriteLoc, name, level, strength, intelligence, dexterity, vitality, textColor, maxDelay, checkText) { }

    public override void OnDefeat() {
        OnKill();
    }

    public abstract Tentacle Summon();
}
