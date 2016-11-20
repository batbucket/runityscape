using UnityEngine;
using System.Collections;

public class Hero : PlayerCharacter {
    public Hero(Inventory items, int strength, int intelligence, int dexterity, int vitality)
        : base(items, "placeholder", "", 1, strength, intelligence, dexterity, vitality, Color.white) {
        this.Attack = new Attack();
        this.AddResource(new NamedResource.Experience(Level));
        this.Actions.Add(new Check());
    }

    public override void Act() {
        base.Act();
    }
}
