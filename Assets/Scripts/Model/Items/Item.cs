using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;

public abstract class Item : SpellFactory {
    public const SpellType SPELL_TYPE = SpellType.BOOST;
    public const TargetType TARGET_TYPE = TargetType.SINGLE_ALLY;
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>();

    public Item(string name, string description) : base(name, description, SPELL_TYPE, TARGET_TYPE, COSTS) { }

    public override string GetNameAndCosts(Character caster) {
        string nameAndCount = string.Format("{0}", this.Name);
        return IsCastable(caster) ? nameAndCount : "<color=red>" + nameAndCount + "</color>";
    }

    public override bool IsCastable(Character caster, Character target = null) {
        return base.IsCastable(caster, target);
    }
}
