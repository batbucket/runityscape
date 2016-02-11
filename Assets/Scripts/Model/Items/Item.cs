using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class Item : Spell {
    public const SpellType SPELL_TYPE = SpellType.BOOST;
    public const SpellTarget TARGET_TYPE = SpellTarget.SINGLE_ALLY;
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>();

    public Item(string name, string description, int count = 1) : base(name, description, SPELL_TYPE, TARGET_TYPE, COSTS) {
        Util.Assert(count > 0);
        this.Count = count;
    }

    public override double CalculateHitRate(Character caster, Character target) {
        return 1;
    }

    public override int CalculateDamage(Character caster, Character target) {
        return Damage = 0;
    }

    protected override void OnFailure(Character caster, Character target) {
        throw new NotImplementedException();
    }

    public override string GetNameAndInfo(Character caster) {
        string nameAndCount = string.Format("{0} x {1}", this.Name, Count);
        return IsCastable(caster) ? nameAndCount : "<color=red>" + nameAndCount + "</color>";
    }

    public override bool IsCastable(Character caster, Character target = null) {
        return base.IsCastable(caster, target) && Count > 0;
    }

    protected override void ConsumeResources(Character caster) {
        base.ConsumeResources(caster);
        caster.Selections[Selection.ITEM].Remove(this);
    }
}
