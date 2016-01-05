using UnityEngine;
using System.Collections;
using System;

public class ComputerCharacter : Character {

    public ComputerCharacter(Sprite sprite, string name, int level, int strength, int intelligence, int dexterity, int vitality)
        : base(sprite, name, level, strength, intelligence, dexterity, vitality) {
    }

    public override void act(int chargeAmount, Game game) {
        charge(chargeAmount);
        if (isCharged()) {
            SpellFactory.createSpell("Attack").initialize(this, game.getPage().getCharacters(true)[0]).tryCast(game);
        }
    }

    public override bool isDefeated(Game game) {
        throw new NotImplementedException();
    }

    public override bool isKilled(Game game) {
        throw new NotImplementedException();
    }

    public override void onDefeat(Game game) {
        throw new NotImplementedException();
    }

    public override void onKill(Game game) {
        throw new NotImplementedException();
    }

    public override void onStart(Game game) {
        throw new NotImplementedException();
    }

    public override void react(Spell spell, Game game) {
        throw new NotImplementedException();
    }
}
