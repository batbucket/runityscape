using Scripts.Model.Characters;
using Scripts.Model.Spells.Named;
using Scripts.Model.Stats.Resources;
using UnityEngine;

public class Hero : PlayerCharacter {

    public Hero()
        : base(new Displays { Loc = "placeholder", Name = "", Color = Color.white, Check = "" }, new StartStats { Str = 2, Agi = 2, Int = 2, Lvl = 1, Vit = 2 }) {
        this.Attack = new Attack();
        this.AddResource(new NamedResource.Experience());
        this.Actions.Add(new Check());
    }

    protected override void Act() {
        base.Act();
    }
}