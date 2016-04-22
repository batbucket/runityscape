using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpellComponent {
    Spell _spell;
    public Spell Spell { get { return _spell; } set { _spell = value; } }
    IDictionary<Result.Type, Result> results;
    public virtual bool IsTimedOut { get { return hasCasted; } set { throw new NotImplementedException(); } }
    bool hasCasted;

    public SpellComponent(Result hit, Result critical = null, Result miss = null) {
        this.results = new SortedDictionary<Result.Type, Result>() {
            { Result.Type.MISS, miss ?? new Result() },
            { Result.Type.CRITICAL, critical ?? new Result() },
            { Result.Type.HIT, hit }
        };
        foreach (KeyValuePair<Result.Type, Result> pair in results) {
            pair.Value.Component = this;
            pair.Value.type = pair.Key;
        }
        this.hasCasted = false;
    }

    public Result DetermineResult() {
        if (results[Result.Type.HIT].IsState()) {
            if (results[Result.Type.CRITICAL].IsState()) {
                return results[Result.Type.CRITICAL];
            } else {
                return results[Result.Type.HIT];
            }
        } else {
            return results[Result.Type.MISS];
        }
    }

    public virtual void Tick() {
        SpellReactions();
        hasCasted = true;
    }


    protected void SpellReactions() {
        Spell.Target.React(Spell);
        foreach (Character c in Game.Instance.PagePresenter.Page.GetAll()) {
            c.Witness(Spell);
        }
    }
}
