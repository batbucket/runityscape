using UnityEngine;
using System.Collections;
using System;

public class Steve : ComputerCharacter {
    public Steve() : base(Util.getSprite("laughing_shinx"), "Steve", 0, 5, 5, 5, 5) {
        addResources(ResourceFactory.createResource(ResourceType.SKILL, 3));
        getFight().Add(new Attack(this));
        getFight().Add(new Meditate(this));
    }

    public override void act(int chargeAmount, Game game) {
        base.act(chargeAmount, game);
        if (getResource(ResourceType.HEALTH).getRatio() < .5) {
            fightSpells[0].setTargets(game.getPage().getCharacters(true).ToArray()[0]).tryCast(game);
        } else {
            fightSpells[0].setTargets(game.getPage().getCharacters(true).ToArray()[0]).tryCast(game);
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

    public override void react(Spell spell, Game game) {
        if (spell.isHit()) {
            game.postText("* Ouch.", Color.green);
        } else {
            game.postText("* lol u missed", Color.green);
        }
    }
}
