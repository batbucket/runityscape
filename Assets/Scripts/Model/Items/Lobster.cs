using UnityEngine;
using System.Collections;
using System;

public class Lobster : ConsumableItem {

    public const string NAME = "Lobster";
    public const string DESCRIPTION = "Eat a Lobster for 12 HP, just like in RuneScape™. Property of Jagex Ltd. The most popular Free-To-Play MMORPG played by millions worldwide.";
    public const int HEAL_AMOUNT = 12;

    public Lobster(int count = 1) : base(NAME, DESCRIPTION, count) { }

    protected override void OnSuccess(Character caster, Character target) {
        caster.AddToResource(ResourceType.HEALTH, false, HEAL_AMOUNT);
        CastText = string.Format("* {0} eats a Lobster for {1} HP!", caster.Name, HEAL_AMOUNT);
    }

    public override void Undo() {
        base.Undo();
        Caster.AddToResource(ResourceType.HEALTH, false, -HEAL_AMOUNT);
    }
}
