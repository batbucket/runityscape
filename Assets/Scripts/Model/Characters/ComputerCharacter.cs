using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class ComputerCharacter : Character {
    public const bool DISPLAYABLE = false;
    readonly float maxDelay;
    protected float delay;

    public ComputerCharacter(Sprite sprite, string name, int level, int strength, int intelligence, int dexterity, int vitality, Color textColor, float maxDelay)
        : base(sprite, name, level, strength, intelligence, dexterity, vitality, textColor, DISPLAYABLE) {
        this.maxDelay = maxDelay;
    }

    protected abstract void DecideSpell();

    protected override void OnFullCharge() {
        delay = UnityEngine.Random.Range(0, maxDelay);
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
                    spell.TryCast(this, page.GetRandomAlly(this.Side));
                    break;
                case TargetType.SINGLE_ENEMY:
                    spell.TryCast(this, page.GetRandomEnemy(this.Side));
                    break;
                case TargetType.SELF:
                    spell.TryCast(this, this);
                    break;
                case TargetType.ALL_ALLY:
                    spell.TryCast(this, page.GetAllies(this.Side));
                    break;
                case TargetType.ALL_ENEMY:
                    spell.TryCast(this, page.GetEnemies(this.Side));
                    break;
                case TargetType.ALL:
                    spell.TryCast(this, page.GetAll());
                    break;
            }
        }
    }
}
