using UnityEngine;
using System.Collections;
using System;

public class Steve : ComputerCharacter {

    public Steve() : base(Util.GetSprite("laughing_shinx"), "Steve", 0, 5, 5, 5, 5, Color.red, 4) {
        AddResources(ResourceFactory.CreateResource(ResourceType.SKILL, 3));
        Spells.Add(new Attack());
    }

    public override void Tick(int chargeAmount, Game game) {
        base.Tick(chargeAmount, game);
    }

    protected override void DecideSpell(Game game) {
        if (GetResource(ResourceType.HEALTH).GetRatio() < .5) {
            QuickCast(new Meditate(this), game);
        } else {
            QuickCast(new Attack(this), game);
        }
    }

    public override bool IsDefeated(Game game) {
        throw new NotImplementedException();
    }

    public override bool IsKilled(Game game) {
        throw new NotImplementedException();
    }

    public override void OnBattleEnd(Game game) {
        throw new NotImplementedException();
    }

    public override void OnDefeat(Game game) {
        throw new NotImplementedException();
    }

    public override void OnKill(Game game) {
        throw new NotImplementedException();
    }

    public override void OnStart(Game game) {
    }

    public override void OnVictory(Game game) {
        throw new NotImplementedException();
    }

    protected override void React(Spell spell, Game game) {
        if (spell.IsTargeting(this) && spell.getResult() != SpellResult.CANT_CAST) {
            switch (spell.getName()) {
                case "Spare":
                    Talk("Don't make me laugh.", game);
                    break;
                case "Surrender":
                    Talk("Thanks!", game);
                    break;
            }
        }
        if (spell.getName().Equals("Surrender")) {
            Talk("Thanks!", game);
        }
    }
}
