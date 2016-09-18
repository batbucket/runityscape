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

    private bool isFirstTick;

    public Spell(SpellFactory spellFactory, Character caster, Character target, SpellDetails other = null) {
        this.SpellFactory = spellFactory;

        this.Hit = spellFactory.CreateHit();
        this.Critical = spellFactory.CreateCritical();
        this.Miss = spellFactory.CreateMiss();

        this.Caster = caster;
        this.Target = target;
        this.Other = other;

        this.isFirstTick = true;
    }

    public bool IsIndefinite;
    public float Duration;
    public float DurationTimer;
    public float TimePerTick;
    public float TickTimer;
    public string DurationText {
        get {
            if (IsIndefinite) {
                return "∞";
            } else {
                return "" + (int)DurationTimer;
            }
        }
    }

    public virtual void Tick() {
        if (isFirstTick) {
            isFirstTick = false;
            Result = DetermineResult();
            IsIndefinite = Result.IsIndefinite(this.Caster, this.Target, this.Other);
            Duration = Result.Duration.Invoke(this.Caster, this.Target, this.Other);
            DurationTimer = Duration;
            TimePerTick = Result.TimePerTick.Invoke(this.Caster, this.Target, this.Other);
            TickTimer = TimePerTick;

            Result.OnStart(this.Caster, this.Target, this.Other);

            if (!IsIndefinite && Duration <= 0 && TimePerTick <= 0) {
                Invoke();
                Target.Buffs.Remove(this);
                IsFinished = true;
                Result.OnEnd(Caster, Target, Other);
            }
        }

        if (!IsIndefinite && !IsFinished && DurationTimer <= 0) {
            Result.OnEnd(Caster, Target, Other);
            Target.Buffs.Remove(this);
            IsFinished = true;
        }

        if (IsIndefinite || (DurationTimer -= Time.deltaTime) > 0) {
            if ((TickTimer -= Time.deltaTime) <= 0) {
                Invoke();
                TickTimer = TimePerTick;
            }
        }
    }

    public Result DetermineResult() {
        if (Hit.IsState.Invoke(this.Caster, this.Target, this.Other)) {
            if (Critical.IsState(this.Caster, this.Target, this.Other)) {
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
        Calculation = Result.Calculation(this.Caster, this.Target, this.Other);

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
        Result.Perform(this.Caster, this.Target, Calculation, this.Other);
        IList<CharacterEffect> ce = Result.Sfx(this.Caster, this.Target, Calculation, this.Other);
        foreach (CharacterEffect sfx in ce) {
            Target.Presenter.PortraitView.AddEffect(sfx);
        }
        Game.Instance.Sound.Play(Result.Sound(this.Caster, this.Target, Calculation, this.Other));
        string text = Result.CreateText(this.Caster, this.Target, Calculation, this.Other);
        if (!string.IsNullOrEmpty(text)) {
            Game.Instance.TextBoxHolder.AddTextBoxView(new TextBox(Result.CreateText(this.Caster, this.Target, Calculation, this.Other), Color.white, TextEffect.FADE_IN));
        }
    }
}
