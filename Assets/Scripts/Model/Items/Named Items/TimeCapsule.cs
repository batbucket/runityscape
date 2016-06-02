using UnityEngine;
using System.Collections;
using System;

public class TimeCapsule : ConsumableItem {
    const string NAME = "Time Capsule";
    const string DESCRIPTION = "A container holding items from the past.";

    public TimeCapsule(int count) : base(NAME, DESCRIPTION, count) { }

    protected override void OnOnce(Character caster, SpellDetails other) {
        caster.Selections[Selection.ITEM].Add(new OldArmor(1));
        caster.Selections[Selection.ITEM].Add(new OldSword(1));
    }

    protected override Calculation GetCalculation(Character caster, Character target) {
        return new Calculation();
    }

    protected override string OtherUseText(Character caster, Character target, Calculation calculation) {
        throw new NotImplementedException();
    }

    protected override string SelfUseText(Character caster, Character target, Calculation calculation) {
        return string.Format("{0} opened the time capsule.", target.Name);
    }
}
