using UnityEngine;
using System.Linq;
using System;

public class Steve : ComputerCharacter {

    public override int ExperienceGiven {
        get {
            return 0;
        }
    }

    public override int GoldGiven {
        get {
            return 0;
        }
    }

    public Steve() : base(new Displays() { Loc = "laughing_shinx", Name = "Steve", Color = Color.white, Check = "Hello world" }, new Attributes() { Lvl = 5, Str = 1, Int = 1, Agi = 1, Vit = 1 }, 10) {
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
