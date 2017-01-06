using Scripts.Model.Characters;
using Scripts.Model.Items.Named;
using Scripts.Model.Spells.Named;
using Scripts.Model.Stats.Resources;
using UnityEngine;

public class Hero : PlayerCharacter {

    public Hero()
        : base(new Displays { Loc = "holy-symbol", Name = "", Color = Color.white, Check = "" }, new StartStats { Str = 1, Agi = 1, Int = 1, Lvl = 1, Vit = 1 }) {
        this.Attack = new Attack();
        this.AddResource(new NamedResource.Experience());
        this.Actions.Add(new Check());
    }

    protected override void Act() {
        base.Act();
    }
}