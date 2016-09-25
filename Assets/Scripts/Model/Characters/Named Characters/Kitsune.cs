using UnityEngine;
using System.Linq;

public class Kitsune : ComputerCharacter {
    public const int STR = 100;
    public const int INT = 200;
    public const int DEX = 50;
    public const int VIT = 50;

    private S s;
    public Kitsune() : base("", "Kitsune", 50, STR, INT, DEX, VIT, Color.white, 1, null) {

    }

    private enum S {
        INTRO,
        FIGHT,
        SUMMON_1,
        SUMMON_2,
        SUMMON_3,
        PUNCH_1,
        PUNCH_2,
    }

    public override void Act() {
        base.Act();
    }

    protected override void DecideSpell() {
        switch (s) {
            case S.INTRO:
                Talk("Are you scared?");
                QuickCast(new PaperFan(1));
                s = S.FIGHT;
                break;
            case S.FIGHT:
                if (!Buffs.Any(b => b.SpellFactory.Name == "BackTurn")) {
                    QuickCast(new BackTurn());
                    Talk("Hehehehe...");
                }
                break;
        }
    }
}
