using Scripts.Model.Characters;
using Scripts.Model.World.Utility;
using Scripts.Model.Spells;
using Scripts.Model.Spells.Named;
using Scripts.Presenter;
using UnityEngine;

public class Shopkeeper : ComputerCharacter {
    private const string NAME = "Maple";
    private EventFlags flags;

    public Shopkeeper(EventFlags flags) : base(new Displays { Loc = "Icons/cowled", Color = Color.yellow, Name = "???", Check = "Checked!" }, new StartStats { Lvl = 3, Str = 2, Int = 2, Agi = 2, Vit = 3 }) {
        this.flags = flags;
    }

    public override void OnBattleStart() {
        if (!flags.Bools[Flag.SHOPKEEPER_FRIENDS] && flags.Bools[Flag.ATTACKED_SHOPKEEPER]) {
            Game.Instance.TextBoxes.AddTextBox(Talk("Oh, you wanna go?"));
            flags.Bools[Flag.ATTACKED_SHOPKEEPER] = true;
        }
    }

    protected override void OnTick() {
        this.Name = flags.Bools[Flag.SHOPKEEPER_GAVE_NAME] ? NAME : "???";
    }

    public override void React(Spell spell) {
    }

    public override void OnVictory() {
        base.OnVictory();
        Game.Instance.TextBoxes.AddTextBox(Talk("Yeah, that's what I thought. Get outta here."));
    }

    public override void OnDefeat() {
        base.OnDefeat();
        if (flags.Bools[Flag.SHOPKEEPER_FRIENDS]) {
            Game.Instance.TextBoxes.AddTextBox(Talk("I thought... we were friends..."));
        } else {
            Game.Instance.TextBoxes.AddTextBox(Talk("..."));
        }
    }

    protected override void DecideSpell() {
        if (!flags.Bools[Flag.SHOPKEEPER_FRIENDS]) {
            QuickCast(new Attack());
        }
    }
}