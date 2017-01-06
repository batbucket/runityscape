using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.Model.TextBoxes;
using Scripts.Presenter;
using Scripts.View.Effects;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Spells {

    /// <summary>
    /// Represents a cast Spell.
    /// SpellFactories create Spells.
    /// </summary>
    public class Spell {
        public float Duration;
        public float DurationTimer;
        public float TickTimer;
        public float TimePerTick;
        private bool isFirstTick;

        public Spell(SpellFactory spellFactory, Character caster, Character target) {
            this.SpellFactory = spellFactory;

            this.Hit = spellFactory.CreateHit();
            this.Critical = spellFactory.CreateCritical();
            this.Miss = spellFactory.CreateMiss();

            this.Caster = caster;
            this.Target = target;

            this.isFirstTick = true;

            Result = DetermineResult();
            Calculation = Result.Calculation(this.Caster, this.Target);
            Duration = Result.Duration.Invoke(this.Caster, this.Target);
            DurationTimer = Duration;
            TimePerTick = Result.TimePerTick.Invoke(this.Caster, this.Target);
            TickTimer = TimePerTick;
        }

        public Calculation Calculation { get; set; }
        public Character Caster { get; set; }
        public Critical Critical { get; set; }

        public string DurationText {
            get {
                if (IsIndefinite) {
                    return "∞";
                } else {
                    return "" + (int)DurationTimer;
                }
            }
        }

        public Hit Hit { get; set; }

        public bool IsIndefinite {
            get {
                return Result.IsIndefinite(Caster, Target);
            }
        }

        public bool IsOneShot {
            get {
                return !IsIndefinite && Duration <= 0 && TimePerTick <= 0;
            }
        }

        public bool IsTimedOut { get; set; }
        public Miss Miss { get; set; }
        public Spell Other { get; set; }
        public Result Result { get; set; }
        public SpellFactory SpellFactory { get; set; }
        public Character Target { get; set; }

        public void Cancel() {
            Result.OnEnd(Caster, Target);
        }

        public Result DetermineResult() {
            if (Hit.IsState.Invoke(this.Caster, this.Target)) {
                if (Critical.IsState(this.Caster, this.Target)) {
                    return Critical;
                } else {
                    return Hit;
                }
            } else {
                return Miss;
            }
        }

        public override bool Equals(object obj) {
            // If parameter is null return false.
            if (obj == null) {
                return false;
            }

            // If parameter cannot be cast to SpellFactory return false.
            Spell s = obj as Spell;
            if ((object)s == null) {
                return false;
            }

            // Return true if the fields match:
            return this.SpellFactory.Equals(s.SpellFactory);
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

            Result.Perform(this.Caster, this.Target, Calculation);

            IList<CharacterEffect> ce = Result.Sfx(this.Caster, this.Target, Calculation);
            foreach (CharacterEffect sfx in ce) {
                Target.Presenter.PortraitView.AddEffect(sfx);
            }

            Game.Instance.Sound.PlaySound(Result.Sound(this.Caster, this.Target, Calculation));
            string text = Result.CreateText(this.Caster, this.Target, Calculation);

            if (!string.IsNullOrEmpty(text) && isFirstTick) {
                Game.Instance.TextBoxes.AddTextBox(new TextBox(Result.CreateText(this.Caster, this.Target, Calculation)));
            }

            Caster.UpdateState();
            Target.UpdateState();

            for (int i = 0; i < characters.Count; i++) {
                Character c = characters[i];
                if (c.State == CharacterState.NORMAL) {
                    c.Witness(this);
                }
            }

            if (Caster.State == CharacterState.NORMAL) {
                Caster.React(this);
            }

            if (Target.State == CharacterState.NORMAL) {
                Target.React(this);
            }
        }

        public virtual void Tick() {
            if (isFirstTick) {
                Result.OnStart(this.Caster, this.Target);
            }

            if (!IsTimedOut) {
                if (!IsIndefinite && DurationTimer <= 0) {
                    Result.OnEnd(Caster, Target);
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
    }
}