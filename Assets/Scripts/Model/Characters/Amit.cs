using UnityEngine;
using System.Collections;
using System;

public class Amit : PlayerCharacter {
    public Amit() : base(Util.getSprite("crying_mudkip"), "Amit", 0, 5, 5, 5, 5) {
        addResources(ResourceFactory.createResource(ResourceType.SKILL, 3));
        fightSpells.Add("Attack");
       // fightSpells.Add("Meditate");
        fightSpells.Add("Lobster");
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

    public override void react(Spell spell, Game game) {

    }
}