using Scripts.Model.Characters;
using Scripts.Model.Spells.Named;
using Scripts.Model.Stats.Resources;
using UnityEngine;

public class Steve : ComputerCharacter {

    public Steve() : base(new Displays() { Loc = "fox-head", Name = "Steve", Color = Color.white, Check = "Hello world" }, new StartStats { Lvl = 5, Str = 1, Int = 1, Agi = 1, Vit = 1 }) {
        AddResource(new NamedResource.Skill());
    }

    protected override void DecideSpell() {
        if ((GetResourceCount(ResourceType.HEALTH, false) + 0.0f) / GetResourceCount(ResourceType.HEALTH, true) < .5f) {
            QuickCast(new Meditate());
        }
    }

    protected override void WhileFullCharge() {
        DecideSpell();
    }
}