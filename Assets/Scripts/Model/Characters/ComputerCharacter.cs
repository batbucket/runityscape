using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class ComputerCharacter : Character {
    public const float CHARGE_CAP_RATIO = 3f;

    public const bool CONTROLLABLE_CPU = false;
    readonly float maxDelay;
    protected float delay;

    public ComputerCharacter(string spriteLoc, string name, int level, int strength, int intelligence, int dexterity, int vitality, Color textColor, float maxDelay, string checkText = "")
        : base(spriteLoc, name, level, strength, intelligence, dexterity, vitality, textColor, CONTROLLABLE_CPU, checkText) {
        this.maxDelay = maxDelay;
        this.delay = UnityEngine.Random.Range(0, maxDelay);
    }

    public override void Act() {
        if (State == CharacterState.ALIVE && IsCharged && (delay -= Time.deltaTime) <= 0) {
            DecideSpell();
            delay = UnityEngine.Random.Range(0, maxDelay);
        }
    }

    public override void CalculateChargeRequirement(Character main) {
        CalculateChargeRequirement(this, main, CHARGE_CAP_RATIO);
    }

    protected abstract void DecideSpell();

    protected void Talk(string s) {
        AvatarBox a = null;
        if (Side) {
            a = new RightBox(this, s);
        } else {
            a = new LeftBox(this, s);
        }
        Game.Instance.AddTextBox(a);
    }

    protected void Emote(string s) {
        Game.Instance.AddTextBox(new TextBox(s));
    }

    protected void Talk(params string[] strings) {
        string s = strings.PickRandom();
        AvatarBox a = null;
        if (Side) {
            a = new RightBox(this, s);
        } else {
            a = new LeftBox(this, s);
        }
        Game.Instance.AddTextBox(a);
    }

    protected void QuickCast(SpellFactory spell, Character target = null) {
        Page page = Game.Instance.PagePresenter.Page;
        if (!spell.IsCastable(this)) {
            return;
        }
        if (target != null) {
            spell.TryCast(this, target);
        } else {
            switch (spell.TargetType) {
                case TargetType.SINGLE_ALLY:
                    Character ally = page.GetRandomAlly(this);
                    spell.TryCast(this, page.GetRandomAlly(this));
                    break;
                case TargetType.SINGLE_ENEMY:
                    spell.TryCast(this, page.GetRandomEnemy(this));
                    break;
                case TargetType.SELF:
                    spell.TryCast(this, this);
                    break;
                case TargetType.ALL_ALLY:
                    spell.TryCast(this, page.GetAllies(this));
                    break;
                case TargetType.ALL_ENEMY:
                    spell.TryCast(this, page.GetEnemies(this));
                    break;
                case TargetType.ALL:
                    spell.TryCast(this, page.GetAll());
                    break;
            }
        }
    }
}
