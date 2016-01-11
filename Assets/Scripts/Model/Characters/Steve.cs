using UnityEngine;
using System.Collections;
using System;

public class Steve : ComputerCharacter {

    public Steve() : base(Util.getSprite("laughing_shinx"), "Steve", 0, 5, 5, 5, 5, Color.red, 4) {
        addResources(ResourceFactory.createResource(ResourceType.SKILL, 3));
        getFight().Add(new Attack(this));
    }

    public override void act(int chargeAmount, Game game) {
        base.act(chargeAmount, game);
    }

    protected override void decideSpell(Game game) {
        if (getResource(ResourceType.HEALTH).getRatio() < .5) {
            quickCast(new Meditate(this), game);
        } else {
            quickCast(new Attack(this), game);
        }
    }

    public override bool isDefeated(Game game) {
        throw new NotImplementedException();
    }

    public override bool isKilled(Game game) {
        throw new NotImplementedException();
    }

    public override void onBattleEnd(Game game) {
        throw new NotImplementedException();
    }

    public override void onDefeat(Game game) {
        throw new NotImplementedException();
    }

    public override void onKill(Game game) {
        throw new NotImplementedException();
    }

    public override void onStart(Game game) {
    }

    public override void onVictory(Game game) {
        throw new NotImplementedException();
    }

    protected override void react(Spell spell, Game game) {
        if (spell.isTarget(this) && spell.getResult() != SpellResult.CANT_CAST) {
            switch (spell.getName()) {
                case "Spare":
                    talk("Don't make me laugh.", game);
                    break;
                case "Surrender":
                    talk("Thanks!", game);
                    break;
            }
        }
        if (spell.getName().Equals("Surrender")) {
            talk("Thanks!", game);
        }
    }
}
