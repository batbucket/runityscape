using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class Spell {
    public SpellFactory SpellFactory { get; set; }
    public Character Caster { get; set; }
    public Character Target { get; set; }
    public SpellDetails Other { get; set; }

    public Hit Hit { get; set; }
    public Critical Critical { get; set; }
    public Miss Miss { get; set; }

    public Result Result { get; set; }
    public Calculation Calculation { get; set; }

    public bool IsFinished { get; set; }

    public Spell(SpellFactory spellFactory, Character caster, Character target, SpellDetails other = null) {
        this.SpellFactory = spellFactory;

        this.Hit = spellFactory.CreateHit();
        this.Critical = spellFactory.CreateCritical();
        this.Miss = spellFactory.CreateMiss();

        this.Hit.Spell = this;
        this.Critical.Spell = this;
        this.Miss.Spell = this;

        this.Caster = caster;
        this.Target = target;
        this.Other = other;
    }

    public virtual void Tick() {
        if (!IsFinished) {
            Invoke();
            IsFinished = true;
        } else {
            Target.Buffs.Remove(this);
        }
    }

    public Result DetermineResult() {
        if (Hit.IsState()) {
            if (Critical.IsState()) {
                return Critical;
            } else {
                return Hit;
            }
        } else {
            return Miss;
        }
    }

    public virtual void React(Spell spell) { }

    public virtual void Witness(Spell spell) { }

    protected virtual void Invoke() {
        Result = DetermineResult();
        Calculation = Result.Calculation();

        Target.React(this);
        for (int i = 0; i < Caster.Buffs.Count; i++) {
            Spell s = Caster.Buffs[i];
            s.React(this);
        }
        for (int i = 0; i < Target.Buffs.Count; i++) {
            Spell s = Target.Buffs[i];
            s.React(this);
        }

        IList<Character> characters = Game.Instance.PagePresenter.Page.GetAll();
        for (int i = 0; i < characters.Count; i++) {
            Character c = characters[i];
            c.Witness(this);
            for (int j = 0; j < c.Buffs.Count; j++) {
                Spell s = c.Buffs[j];
                s.Witness(this);
            }
        }
        Result.Effect(Calculation);
        IList<CharacterEffect> ce = Result.SFX(Calculation);
        foreach (CharacterEffect sfx in ce) {
            Target.Presenter.PortraitView.AddEffect(sfx);
        }
        Game.Instance.Sound.Play(Result.Sound(Calculation));
        string text = Result.CreateText(Calculation);
        if (!string.IsNullOrEmpty(text)) {
            Game.Instance.TextBoxHolder.AddTextBoxView(new TextBox(Result.CreateText(Calculation), Color.white, TextEffect.FADE_IN));
        }
    }
}
