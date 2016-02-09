using UnityEngine;
using System.Collections;
using System;

public class Amit : PlayerCharacter {
    public Amit() : base(Util.GetSprite("crying_mudkip"), "Amit", 0, 5, 5, 5, 5, Color.white) {
        Side = false;
        AddResources(ResourceFactory.CreateResource(ResourceType.SKILL, 3));
        this.Selections[Selection.SPELL].Add(new Attack());
        this.Selections[Selection.SPELL].Add(new Meditate());
        this.Selections[Selection.SPELL].Add(new Spare());
        this.Selections[Selection.ITEM].Add(new Lobster(2));
    }

    public override bool IsDefeated() {
        throw new NotImplementedException();
    }

    public override bool IsKilled() {
        throw new NotImplementedException();
    }

    public override void OnBattleEnd() {
        throw new NotImplementedException();
    }

    public override void OnDefeat() {
        throw new NotImplementedException();
    }

    public override void OnKill() {
        throw new NotImplementedException();
    }

    public override void OnStart() {
        throw new NotImplementedException();
    }

    public override void OnVictory() {
        throw new NotImplementedException();
    }

    protected override void OnFullCharge() {

    }
}