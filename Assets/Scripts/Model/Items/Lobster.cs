using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Lobster : ConsumableItem {

    public const string NAME = "Lobster";
    public const string DESCRIPTION = "Eat a Lobster for 12 HP, just like in RuneScape™. Property of Jagex Ltd. The most popular Free-To-Play MMORPG played by millions worldwide.";
    public const int HEAL_AMOUNT = 12;

    public const string USE_TEXT = "{0} ate a Lobster, restoring {1} life!";

    public Lobster(int count = 1) : base(NAME, DESCRIPTION, count) { }

    protected override IDictionary<string, SpellComponent> CreateComponents(Character caster, Character target, Spell spell) {
        throw new NotImplementedException();
    }
}
