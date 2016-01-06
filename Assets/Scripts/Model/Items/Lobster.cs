using UnityEngine;
using System.Collections;
using System;

public class Lobster : ConsumableItem {


    public const string NAME = "Lobster";
    public const int HEAL_AMOUNT = 12;

    public Lobster(Character caster) : base(caster, NAME)
    {

    }

    public override void onSuccess(Game game)
    {
        base.onSuccess(game);
        caster.addToResource(ResourceType.HEALTH, false, HEAL_AMOUNT);
        game.postText("HURRDURR IM EATING A LOBSTA");
    }

    public override void undo(Game game)
    {
        base.undo(game);
        caster.addToResource(ResourceType.HEALTH, false, -HEAL_AMOUNT);
    }
}
