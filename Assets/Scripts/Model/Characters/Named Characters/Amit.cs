using UnityEngine;
using System.Collections;
using System;

public class Amit : PlayerCharacter {
    public Amit() : base("crying_mudkip", "Amit", 0, 4, 5, 2, 5, Color.white) {
        Side = false;
        AddResource(new NamedResource.Skill());
        this.Attack = new Attack();
        this.Spells.Add(new Meditate());
        this.Actions.Add(new Check());
        this.Items.Add(new Lobster());
        this.Items.Add(new Lobster());
        this.Items.Add(new Scimitar());
        this.Mercies.Add(new Spare());
        this.Spells.Add(new Smite());
        this.Spells.Add(new Poison());
        this.Spells.Add(new Petrify());
    }
}