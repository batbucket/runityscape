using UnityEngine;
using System.Collections;
using System;

public class Amit : PlayerCharacter {
    public Amit() : base(new Displays { Loc = "crying_mudkip", Name = "Amit", Color = Color.white, Check = "Yourself" }, new Attributes { Lvl = 2, Str = 5, Agi = 5, Int = 5, Vit = 5 }) {
        Side = false;
        AddResource(new NamedResource.Skill());
        AddResource(new NamedResource.Experience());
        this.Attack = new Attack();
        this.Spells.Add(new Meditate());
        this.Actions.Add(new Check());
        this.Inventory.Add(new Lobster());
        this.Inventory.Add(new Lobster());
        this.Inventory.Add(new Scimitar());
        this.Mercies.Add(new Spare());
        this.Spells.Add(new Smite());
        this.Spells.Add(new Poison());
        this.Spells.Add(new Petrify());
    }
}