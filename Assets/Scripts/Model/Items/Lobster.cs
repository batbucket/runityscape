using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Lobster : ConsumableItem {

    public const string NAME = "Lobster";
    public static readonly string DESCRIPTION = string.Format("Eat a Lobster for {0} HP, just like in RuneScape™. Property of Jagex Ltd. The most popular Free-To-Play MMORPG played by millions worldwide.", HEAL_AMOUNT);
    public const int HEAL_AMOUNT = 12;
    public const string USE_TEXT_SELF = "* {1} ate a Lobster, restoring {2} life!";
    public const string USE_TEXT_OTHER = "* {0} fed a Lobster to {1}, restoring {2} life!";

    public Lobster(int count = 1) : base(NAME, DESCRIPTION, count) { }

    protected override Calculation GetCalculation(Character caster, Character target) {
        return new Calculation(targetResources: new Dictionary<ResourceType, PairedInt>() { { ResourceType.HEALTH, new PairedInt(0, HEAL_AMOUNT) } });
    }

    protected override string SelfUseText(Character caster, Character target, Calculation calculation) {
        return string.Format(USE_TEXT_SELF, caster.Name, target.Name, calculation.TargetResources[ResourceType.HEALTH].False);
    }

    protected override string OtherUseText(Character caster, Character target, Calculation calculation) {
        return string.Format(USE_TEXT_OTHER, caster.Name, target.Name, calculation.TargetResources[ResourceType.HEALTH].False);
    }
}
