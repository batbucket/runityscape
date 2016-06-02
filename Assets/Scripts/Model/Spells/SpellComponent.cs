using System;
using System.Collections.Generic;
using UnityEngine;

public class SpellComponent : IReactable {
    Spell _spell;
    public Spell Spell { get { return _spell; } set { _spell = value; } }
    IDictionary<Result.Type, Result> results;

    bool done;
    public virtual bool IsFinished { get { return done; } }

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
    }

    public virtual void React(Spell spell, Result res, Calculation calc) { }
    public virtual bool IsReactOverride(Spell s, Result r) { return false; }

    public virtual void Witness(Spell spell, Result res, Calculation calc) { }
    public virtual bool IsWitnessOverride(Spell spell, Result res) { return false; }

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

    protected virtual void Invoke() {
        Result result = DetermineResult();
        Calculation calc = result.Calculation();

        Spell.Target.React(Spell, result, calc);
        for (int i = 0; i < Spell.Caster.Buffs.Count; i++) {
            Spell s = Spell.Caster.Buffs[i];
            s.Current.React(Spell, result, calc);
        }
        for (int i = 0; i < Spell.Target.Buffs.Count; i++) {
            Spell s = Spell.Target.Buffs[i];
            s.Current.React(Spell, result, calc);
        }

        IList<Character> characters = Game.Instance.PagePresenter.Page.GetAll();
        for (int i = 0; i < characters.Count; i++) {
            Character c = characters[i];
            c.Witness(Spell, result, calc);
            for (int j = 0; j < c.Buffs.Count; j++) {
                Spell s = c.Buffs[j];
                s.Current.Witness(Spell, result, calc);
            }
        }
        result.Effect(calc);
        result.SFX(calc);
        string text = result.CreateText(calc);
        if (!string.IsNullOrEmpty(text)) {
            Game.Instance.TextBoxHolder.AddTextBoxView(new TextBox(result.CreateText(calc), Color.white, TextEffect.FADE_IN));
        }
    }

    public virtual void Tick() {
        Invoke();
        done = true;
    }
}
