using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;

public abstract class Item : SpellFactory {
    public const SpellType SPELL_TYPE = SpellType.BOOST;
    public const TargetType TARGET_TYPE = TargetType.SINGLE_ALLY;

    public Item(string name, string description) : base(name, description, SPELL_TYPE, TARGET_TYPE) { }

    public override string GetNameAndCosts(Character caster) {
        string nameAndCount = string.Format("{0}", this.Name);
        return IsCastable(caster) ? nameAndCount : "<color=red>" + nameAndCount + "</color>";
    }
}
