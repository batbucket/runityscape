using UnityEngine;
using System.Collections;
using System;

public class Steve : ComputerCharacter {

    public Steve() : base(Util.GetSprite("laughing_shinx"), "Steve", 0, 5, 5, 5, 5, Color.red, 4) {
        AddResources(ResourceFactory.CreateResource(ResourceType.SKILL, 3));
        Selections[Selection.SPELL].Add(new Attack());
    }

    public override void Act() {

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

    public override void React(Spell spell) {
        throw new NotImplementedException();
    }

    protected override void DecideSpell() {
        throw new NotImplementedException();
    }
}
