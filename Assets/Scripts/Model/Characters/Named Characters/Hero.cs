using UnityEngine;
using System.Collections;

public class Hero : PlayerCharacter {
    public Hero(Inventory items)
        : base(items, new Displays { Loc = "placeholder", Name = "", Color = Color.white, Check = "" }, new Attributes { Str = 1, Agi = 1, Int = 1, Lvl = 1, Vit = 1 }) {
        this.Attack = new Attack();
        this.AddResource(new NamedResource.Experience(Level));
        this.Actions.Add(new Check());
    }

    protected override void Act() {
        base.Act();
    }
}
