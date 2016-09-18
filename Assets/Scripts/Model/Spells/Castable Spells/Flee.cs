using UnityEngine;
using System.Collections;
using System;

public class Flee : SpellFactory {

    private Page escapePage;
    public Flee(Page escapePage) : base("Flee", "Escape from battle.", SpellType.MERCY, TargetType.SELF) {
        this.escapePage = escapePage;
    }

    public override Hit CreateHit() {
        return new Hit(
            isState: (c, t, o) => true,
            perform: (c, t, calc, o) => Game.Instance.StartCoroutine(Escape()),
            createText: (c, t, calc, o) => string.Format("{0} fled from battle!", t.DisplayName)
            );
    }

    private IEnumerator Escape() {
        yield return new WaitForSeconds(0.5f);
        Game.Instance.PagePresenter.SetPage(escapePage);
    }
}
