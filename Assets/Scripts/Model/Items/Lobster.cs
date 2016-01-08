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
        setCastText(string.Format("* {0} eats a Lobster for {1} HP!", caster.getName(), HEAL_AMOUNT));
    }

    public override void undo() {
        base.undo();
        caster.getInventory().add(this);
        caster.addToResource(ResourceType.HEALTH, false, -HEAL_AMOUNT);
    }
}
