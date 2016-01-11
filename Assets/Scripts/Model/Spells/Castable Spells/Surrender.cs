using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Surrender : Spell {
    public const string NAME = "Surrender";
    public const string DESCRIPTION = "THIS SPELL SHOULD BE REMOVED BECAUSE YOU CAN RUN AWAY.";
    public const SpellType SPELL_TYPE = SpellType.MERCY;
    public const TargetType TARGET_TYPE = TargetType.SELF;
    public const string SUCCESS_CAST = "* {0} surrenders.";
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>();
    int amount;

    public Surrender(Character caster) : base(caster, NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) {
    }

    public override void onFailure() {
        throw new NotImplementedException();
    }

    public override void onSuccess() {
        Util.assert(targets.Count == 1);
        setCastText(string.Format(SUCCESS_CAST, caster));
    }

    public override void undo() {
        throw new NotImplementedException();
    }
}
