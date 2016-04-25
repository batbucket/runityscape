using UnityEngine;
using System.Collections;
using System;

public class Steve : ComputerCharacter {

    public Steve() : base(Util.GetSprite("laughing_shinx"), "Steve", 0, 10, 2, 5, 5, Color.red, 4) {
        AddResource(ResourceType.SKILL, 3);
        Selections[Selection.SPELL].Add(new Attack());
    }

    public override void Act() {
    }

    protected override void DecideSpell() {
        if (Game.Instance.PagePresenter.Page.GetEnemies(Side).Count > 0 && (delay -= Time.deltaTime) <= 0) {
            if ((new Meditate()).IsCastable(this) && Resources[ResourceType.HEALTH].GetRatio() < .5f) {
                QuickCast(new Meditate(), this);
            } else {
                QuickCast(new Attack(), Game.Instance.PagePresenter.Page.GetRandomEnemy(Side));
            }
        }
    }

    protected override void WhileFullCharge() {
        DecideSpell();
    }
}
