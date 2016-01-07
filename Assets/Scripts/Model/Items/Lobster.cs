using UnityEngine;
using System.Collections;
using System;

public class Lobster : ConsumableItem {


    public const string NAME = "Lobster";
    public const int HEAL_AMOUNT = 12;

    public Lobster(Character caster) : base(caster, NAME) {

    }

    public override void onSuccess() {
        base.onSuccess();
        caster.addToResource(ResourceType.HEALTH, false, HEAL_AMOUNT);
        setCastText("HURRDURR IM EATING A LOBSTA");
    }

    public override void undo() {
        base.undo();
        caster.addToResource(ResourceType.HEALTH, false, -HEAL_AMOUNT);
    }
}
