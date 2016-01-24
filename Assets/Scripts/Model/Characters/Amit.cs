using UnityEngine;
using System.Collections;
using System;

public class Amit : PlayerCharacter {
    public Amit() : base(Util.GetSprite("crying_mudkip"), "Amit", 0, 5, 5, 5, 5, Color.white) {
        side = false;
        AddResources(ResourceFactory.CreateResource(ResourceType.SKILL, 3));
        fightSpells.Add(new Attack(this));
        fightSpells.Add(new Meditate(this));
        mercySpells.Add(new Spare(this));
        mercySpells.Add(new Surrender(this));
        inventory.add(new Lobster(this, 2));
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

    public override void OnVictory(Game game) {
        throw new NotImplementedException();
    }

    protected override void OnFullCharge(Game game) {

    }

    protected override void React(Spell spell, Game game) {

    }
}