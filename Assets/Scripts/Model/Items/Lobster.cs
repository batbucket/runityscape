using UnityEngine;
using System.Collections;
using System;

public class Lobster : ConsumableItem {

    public const string NAME = "Lobster";
    public const string DESCRIPTION = "Eat a Lobster for 12 HP, just like in RuneScape™. Property of Jagex Ltd. The most popular Free-To-Play MMORPG played by millions worldwide.";
    public const int HEAL_AMOUNT = 12;

    public const string USE_TEXT = "{0} ate a Lobster, restoring {1} life!";

    public Lobster(int count = 1) : base(NAME, DESCRIPTION, count) { }

    protected override void OnHitCalculation(Spell spell) {
        spell.Resources[ResourceType.HEALTH].False = HEAL_AMOUNT;
    }

    public override void Undo(Spell spell) {
        spell.Caster.Selections[Selection.ITEM].Add(new Lobster());
    }

    protected override string OnHitText(Spell spell) {
        return string.Format(USE_TEXT, spell.Caster, HEAL_AMOUNT);
    }
}
