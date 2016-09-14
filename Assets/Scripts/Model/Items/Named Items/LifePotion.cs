using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LifePotion : ConsumableItem {

    public const string NAME = "LifePotion";
    public static readonly string DESCRIPTION = string.Format("Heal an ally for <color=lime>10</color> Life.", HEAL_AMOUNT);
    public const int HEAL_AMOUNT = 5;
    public const string USE_TEXT_SELF = "{1} uses a life potion, restoring <color=lime>{2}</color> life!";
    public const string USE_TEXT_OTHER = "{0} uses a life potion on {1}, restoring <color=lime>{2}</color> life!";

    public LifePotion(int count = 1) : base(NAME, DESCRIPTION, count) { }

    protected override Calculation CreateCalculation(Character caster, Character target) {
        return new Calculation(targetResources: new Dictionary<ResourceType, PairedInt>() { { ResourceType.HEALTH, new PairedInt(0, HEAL_AMOUNT) } });
    }

    protected override string SelfUseText(Character caster, Character target, Calculation calculation) {
        return string.Format(USE_TEXT_SELF, caster.DisplayName, target.DisplayName, calculation.TargetResources[ResourceType.HEALTH].False);
    }

    protected override string OtherUseText(Character caster, Character target, Calculation calculation) {
        return string.Format(USE_TEXT_OTHER, caster.DisplayName, target.DisplayName, calculation.TargetResources[ResourceType.HEALTH].False);
    }
}
