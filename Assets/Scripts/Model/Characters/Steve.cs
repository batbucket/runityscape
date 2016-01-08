using UnityEngine;
using System.Collections;
using System;

public class Steve : ComputerCharacter {

    public Steve() : base(Util.getSprite("laughing_shinx"), "Steve", 0, 5, 5, 5, 5, Color.red, 4) {
        side = true;
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
        if (spell.isTarget(this)) {
            switch (spell.getResult()) {
                case SpellResult.HIT:
                    game.postText("* Ouch.", Color.green);
                    break;
                case SpellResult.MISS:
                    game.postText("* lol u missed", Color.green);
                    break;
                case SpellResult.CANT_CAST:
                    //game.postText
                    break;
            }
        }
    }
}
