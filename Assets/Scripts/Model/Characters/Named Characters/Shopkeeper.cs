using UnityEngine;
using System.Collections;
using System;

public class Shopkeeper : ComputerCharacter {
    private const string NAME = "Maple";
    private Flags flags;

    public override int ExperienceGiven {
        get {
            return 20;
        }
    }

    public override int GoldGiven {
        get {
            return Util.Random(50, .1f);
        }
    }

    public Shopkeeper(Flags flags) : base(new Displays { Loc = "Icons/cowled", Color = Color.yellow, Name = "???", Check = "Checked!" }, new Attributes { Lvl = 3, Str = 2, Int = 2, Agi = 2, Vit = 3 }, 3) {
        this.flags = flags;
    }

    protected override void OnTick() {
        this.Name = flags.Bools[Flag.SHOPKEEPER_GAVE_NAME] ? NAME : "???";
    }

    protected override void DecideSpell() {
    }
}
