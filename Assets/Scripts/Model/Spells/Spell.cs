using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class Spell {
    public SpellFactory SpellFactory { get; set; }
    public Character Caster { get; set; }
    public Character Target { get; set; }
    public Spell Other { get; set; }

    public Hit Hit { get; set; }
    public Critical Critical { get; set; }
    public Miss Miss { get; set; }

    public Result Result { get; set; }
    public Calculation Calculation { get; set; }

    public bool IsTimedOut { get; set; }

    private bool isFirstTick;

    public bool IsOneShot {
        get {
            return !IsIndefinite && Duration <= 0 && TimePerTick <= 0;
        }
    }

    public Spell(SpellFactory spellFactory, Character caster, Character target, Spell other = null) {
        this.SpellFactory = spellFactory;

        this.Hit = spellFactory.CreateHit();
        this.Critical = spellFactory.CreateCritical();
        this.Miss = spellFactory.CreateMiss();

        this.Caster = caster;
        this.Target = target;
        this.Other = other;

        this.isFirstTick = true;

        Result = DetermineResult();
        Calculation = Result.Calculation(this.Caster, this.Target, this.Other);
        Duration = Result.Duration.Invoke(this.Caster, this.Target, this.Other);
        DurationTimer = Duration;
        TimePerTick = Result.TimePerTick.Invoke(this.Caster, this.Target, this.Other);
        TickTimer = TimePerTick;
    }

    public bool IsIndefinite {
        get {
            return Result.IsIndefinite(Caster, Target, this.Other);
        }
    }
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
            Result.OnStart(this.Caster, this.Target, this.Other);
        }

        if (!IsTimedOut) {
            if (!IsIndefinite && DurationTimer <= 0) {
                Result.OnEnd(Caster, Target, Other);
                IsTimedOut = true;
            }

            if (IsIndefinite || (DurationTimer -= Time.deltaTime) > 0) {
                if ((TickTimer -= Time.deltaTime) <= 0) {
                    Invoke();
                    TickTimer = TimePerTick;
                    isFirstTick = false;
                }
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

    public virtual void Invoke() {
        for (int i = 0; i < Caster.Buffs.Count; i++) {
            Spell buff = Caster.Buffs[i];
            buff.Result.React(this);
        }
        for (int i = 0; i < Target.Buffs.Count; i++) {
            Spell buff = Target.Buffs[i];
            buff.Result.React(this);
        }

        IList<Character> characters = Game.Instance.CurrentPage.GetAll();
        for (int i = 0; i < characters.Count; i++) {
            Character c = characters[i];
            c.Witness(this);
            for (int j = 0; j < c.Buffs.Count; j++) {
                Spell buff = c.Buffs[j];
                buff.Result.Witness(this);
            }
        }

        Result.Perform(this.Caster, this.Target, Calculation, this.Other);

        IList<CharacterEffect> ce = Result.Sfx(this.Caster, this.Target, Calculation, this.Other);
        foreach (CharacterEffect sfx in ce) {
            Target.Presenter.PortraitView.AddEffect(sfx);
        }

        Game.Instance.Sound.Play(Result.Sound(this.Caster, this.Target, Calculation, this.Other));
        string text = Result.CreateText(this.Caster, this.Target, Calculation, this.Other);

        if (!string.IsNullOrEmpty(text) && isFirstTick) {
            Game.Instance.TextBoxes.AddTextBox(new TextBox(Result.CreateText(this.Caster, this.Target, Calculation, this.Other), TextEffect.FADE_IN));
        }

        Caster.UpdateState();
        Target.UpdateState();

        for (int i = 0; i < characters.Count; i++) {
            Character c = characters[i];
            c.Witness(this);
        }

        Caster.React(this);
        Target.React(this);
    }

    public void Cancel() {
        Result.OnEnd(Caster, Target, Other);
    }
}
