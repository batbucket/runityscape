using UnityEngine;
using System.Collections;
using System;

public class Amit : PlayerCharacter {
    public Amit() : base(Util.getSprite("crying_mudkip"), "Amit", 0, 5, 5, 5, 5, Color.white) {
        side = false;
        addResources(ResourceFactory.createResource(ResourceType.SKILL, 3));
        fightSpells.Add(new Attack(this));
        fightSpells.Add(new Meditate(this));
        inventory.add(new Lobster(this, 2));
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

    public override void onVictory(Game game) {
        throw new NotImplementedException();
    }

    protected override void onFullCharge(Game game) {

    }

    protected override void react(Spell spell, Game game) {

    }
}