using UnityEngine;
using System.Collections;

public class Hero : PlayerCharacter {
    public Hero()
        : base(new Displays { Loc = "placeholder", Name = "", Color = Color.white, Check = "" }, new Attributes { Str = 2, Agi = 2, Int = 2, Lvl = 1, Vit = 2 }) {
        this.Attack = new Attack();
        this.AddResource(new NamedResource.Experience());
        this.Actions.Add(new Check());
        this.Equipment.Add(new OldArmor());
        this.Equipment.Add(new OldSword());
    }

    protected override void Act() {
        base.Act();
    }
}
