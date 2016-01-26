using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class ComputerCharacter : Character {
    public const bool DISPLAYABLE = false;
    readonly float maxDelay;
    float delay;

    public ComputerCharacter(Sprite sprite, string name, int level, int strength, int intelligence, int dexterity, int vitality, Color textColor, float maxDelay)
        : base(sprite, name, level, strength, intelligence, dexterity, vitality, textColor, DISPLAYABLE) {
        this.maxDelay = maxDelay;
    }

    public override void Act(int chargeAmount, Game game) {
        base.Act(chargeAmount, game);
        if ((delay -= Time.deltaTime) <= 0) {
            DecideSpell(game);
        }
    }

    protected abstract void DecideSpell(Game game);

    protected override void OnFullCharge(Game game) {
        delay = UnityEngine.Random.Range(0, maxDelay);
    }

    protected void QuickCast(Spell spell, Character target, Game game) {
        spell.setTarget(target);
        CastAndPost(spell, game);
    }

    protected void QuickCast(Spell spell, Game game, Character target = null) {
        if (!spell.canCast()) {
            return;
        }
        if (target != null) {
            spell.setTarget(target);
        } else {
            switch (spell.getTargetType()) {
                case TargetType.SINGLE_ALLY:
                    spell.setTarget(game.getRandomAlly(side));
                    break;
                case TargetType.SINGLE_ENEMY:
                    spell.setTarget(game.getRandomEnemy(side));
                    break;
                case TargetType.SELF:
                    spell.setTarget(this);
                    break;
                case TargetType.ALL_ALLY:
                    spell.setTargets(game.getAllies(side));
                    break;
                case TargetType.ALL_ENEMY:
                    spell.setTargets(game.getEnemies(side));
                    break;
                case TargetType.ALL:
                    spell.setTargets(game.GetAll());
                    break;
            }
        }
        CastAndPost(spell, game);
        GetReactions(spell, game);
    }

    void CastAndPost(Spell spell, Game game) {
        spell.TryCast();
        if (spell.getResult() != SpellResult.CANT_CAST) {
            game.postText(spell.getCastText());
        }
    }
}
