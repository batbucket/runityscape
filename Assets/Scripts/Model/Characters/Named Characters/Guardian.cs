using UnityEngine;
using System.Collections;
using System;

public class Guardian : ComputerCharacter {
    enum State {
        TELL_ATTACK,
        REACT_ATTACK,
        TELL_WIELD,
    }

    State state;

    public Guardian() : base(Util.GetSprite("placeholder"), "Alestre", 100, 100, 100, 100, 100, Color.yellow, 0) {
        state = State.TELL_ATTACK;
    }

    public override void Act() {
        base.Act();
        switch (state) {
            case State.TELL_ATTACK:
                Game.Instance.OrderedEvents(
                    new Game.Event(new RightBox(this, string.Format("Do not hold back, Redeemer {0}. Attack me with all you've got!", Game.Instance.MainCharacter.Name)), 1),
                    new Game.Event(() => {
                        Game.Instance.MainCharacter.Attack = new Attack();
                        Game.Instance.Sound.Play("Music/Hero Immortal");
                    }, 0)
                );
                state = State.REACT_ATTACK;
                break;
        }
    }

    public override void React(Spell spell, Result res, Calculation calc) {
        if (spell.SpellFactory.Name == "Attack" && state == State.REACT_ATTACK) {
            TextBox t = null;
            switch (res.type) {
                case Result.Type.MISS:
                    t = new RightBox(this, "Did you <i>truly</i> put all your heart into that attack?");
                    break;
                case Result.Type.HIT:
                    t = new RightBox(this, "No hesitation, and yet, so little of an effect.");
                    break;
                case Result.Type.CRITICAL:
                    t = new RightBox(this, "Aimed straight for the neck, and yet... so little of an effect.");
                    break;
            };
            Game.Instance.OrderedEvents(
                new Game.Event(t, 1),
                new Game.Event(() => {
                    Game.Instance.AddTextBox(new RightBox(this, "Perhaps you would have more confidence if you dressed the part."));
                }
                )
            );
            state = State.TELL_WIELD;
        }
    }

    public override void OnBattleStart() {
        base.OnBattleStart();
    }

    public override void OnBattleEnd() {
        base.OnBattleEnd();
    }

    protected override void DecideSpell() {
        //throw new NotImplementedException();
    }

    protected override void WhileFullCharge() {
        //throw new NotImplementedException();
    }
}
