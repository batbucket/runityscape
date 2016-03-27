using UnityEngine;
using System.Collections;
using System;

public class Steve : ComputerCharacter {

    public Steve() : base(Util.GetSprite("laughing_shinx"), "Steve", 0, 10, 2, 5, 20, Color.red, 4) {
        AddResource(ResourceType.SKILL, 3);
        Selections[Selection.SPELL].Add(new Attack());
    }

    public override void Act() {
    }

    public override bool IsDefeated() {
        throw new NotImplementedException();
    }

    public override bool IsKilled() {
        return Resources[ResourceType.HEALTH].False <= 0;
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

    protected override void DecideSpell() {
        throw new NotImplementedException();
    }

    protected override void WhileFullCharge() {
        if (Game.Instance.PagePresenter.Page.GetEnemies(Side).Count > 0 && (delay -= Time.deltaTime) <= 0) {
            if ((new Meditate()).IsCastable(this) && Resources[ResourceType.HEALTH].GetRatio() < .5f) {
                QuickCast(new Meditate(), this);
            } else {
                QuickCast(new Attack(), Game.Instance.PagePresenter.Page.GetRandomEnemy(Side));
            }
        }
    }
}
