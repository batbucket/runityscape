using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Spare : Spell {
    public const string NAME = "Spare";
    public const string DESCRIPTION = "Attempt to spare your foe.";
    public const SpellType SPELL_TYPE = SpellType.MERCY;
    public const SpellTarget TARGET_TYPE = SpellTarget.SINGLE_ENEMY;
    public const string SUCCESS_CAST = "* {0} is sparing {1}.";
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>();
    int amount;

    public Spare() : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) { }

    public override double CalculateHitRate(Character caster, Character target) {
        return 1;
    }

    public override int CalculateDamage(Character caster, Character target) {
        return 0;
    }

    protected override void OnSuccess(Character caster, Character target) {
    }

    protected override void OnFailure(Character caster, Character target) {
    }

    public override void Undo() {
    }
}
