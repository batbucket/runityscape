using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class ComputerCharacter : Character {

    public ComputerCharacter(Sprite sprite, string name, int level, int strength, int intelligence, int dexterity, int vitality)
        : base(sprite, name, level, strength, intelligence, dexterity, vitality) {
    }

    public override void act(int chargeAmount, Game game) {
        charge(chargeAmount);
    }

    protected void quickCast(Spell spell, Character target, Game game) {
        spell.setTarget(target);
        spell.tryCast();
    }

    protected void quickCast(Spell spell, Game game, Character target = null) {
        if (target != null) {

        } else {
            List<Character> enemies = game.getEnemies(side);
            List<Character> allies = game.getAllies(side);
            List<Character> allChars = game.getAll();
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
                    spell.setTargets(game.getAll());
                    break;
            }
        }
        spell.tryCast();
    }
}
