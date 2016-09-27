using UnityEngine;
using System.Collections;
using System;

public class TimeCapsule : ConsumableItem {
    const string NAME = "TimeCapsule";
    const string DESCRIPTION = "A container holding items from the past.";
    readonly Item[] items;

    public TimeCapsule(int count) : base(NAME, DESCRIPTION, count) {
        items = new Item[] {
            new OldSword(1),
            new OldArmor(1),
            new LifePotion(3)
        };
    }

    protected override void OnOnce(Character caster, Spell other) {
        foreach (Item i in items) {
            caster.Selections[Selection.ITEM].Add(i);
        }
    }

    protected override Calculation CreateCalculation(Character caster, Character target) {
        return new Calculation();
    }

    protected override string OtherUseText(Character caster, Character target, Calculation calculation) {
        throw new NotImplementedException();
    }

    protected override string SelfUseText(Character caster, Character target, Calculation calculation) {
        return string.Format("{0} opened the time capsule.\nOld equipment has been added to {0}'s Items.\nEquipping them may help {0} recall something...", target.DisplayName);
    }
}
