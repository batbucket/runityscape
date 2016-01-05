using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Attack : Spell {
    public const string NAME = "Attack";
    public const SpellType SPELL_TYPE = SpellType.OFFENSE;
    public const TargetType TARGET_TYPE = TargetType.SINGLE;
    public const string CAST_TEXT = "* {0} attacks {1}.\n{1} takes {2} damage!";
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>() {
        { ResourceType.CHARGE, 100 }
    };

    int amount;

    public Attack() : base(NAME, SPELL_TYPE, TARGET_TYPE, CAST_TEXT, COSTS) {
    }

    //damages for 100% of strength
    protected override void cast(Game game) {
        Debug.Assert(targets.Count == 1);
        amount = caster.getAttribute(AttributeType.STRENGTH).getFalse();
        targets[0].getResource(ResourceType.HEALTH).subtractFalse(amount);
        game.postText(string.Format(CAST_TEXT, caster.getName(), targets[0].getName(), amount));
    }

    public override void undo(Game game) {
        targets[0].getResource(ResourceType.HEALTH).addFalse(amount);
    }
}
