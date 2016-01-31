using UnityEngine;
using System.Collections;
using System;

public class Steve : ComputerCharacter {

    public Steve() : base(Util.GetSprite("laughing_shinx"), "Steve", 0, 5, 5, 5, 5, Color.red, 4) {
        AddResources(ResourceFactory.CreateResource(ResourceType.SKILL, 3));
        Selections[Selection.SPELL].Add(new Attack());
    }

    public override void Act(Page page) {
        throw new NotImplementedException();
    }

    public override bool IsDefeated() {
        throw new NotImplementedException();
    }

    public override bool IsKilled() {
        throw new NotImplementedException();
    }

    public override void OnBattleEnd(Page page) {
        throw new NotImplementedException();
    }

    public override void OnDefeat(Page page) {
        throw new NotImplementedException();
    }

    public override void OnKill(Page page) {
        throw new NotImplementedException();
    }

    public override void OnStart(Page page) {
        throw new NotImplementedException();
    }

    public override void OnVictory(Page page) {
        throw new NotImplementedException();
    }

    public override void React(Spell spell, Page page) {
        throw new NotImplementedException();
    }

    protected override void DecideSpell() {
        throw new NotImplementedException();
    }
}
