using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Spare : Spell {
    public const string NAME = "Spare";
    public const string DESCRIPTION = "Attempt to spare your foe.";
    public const SpellType SPELL_TYPE = SpellType.MERCY;
    public const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
    public const string SUCCESS_CAST = "* {0} is sparing {1}.";
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>();
    int amount;

    public Spare(Character caster) : base(caster, NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) {
    }

    public override void onFailure() {
        throw new NotImplementedException();
    }

    public override void onSuccess() {
        Util.assert(targets.Count == 1);
    }

    public override void undo() {
        throw new NotImplementedException();
    }
}
