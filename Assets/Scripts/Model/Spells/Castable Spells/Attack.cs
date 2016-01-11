using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Attack : Spell {
    public const string NAME = "Attack";
    public const string DESCRIPTION = "Attack a single enemy for STR damage.";
    public const SpellType SPELL_TYPE = SpellType.OFFENSE;
    public const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
    public const string SUCCESS_CAST = "* {0} attacks {1} for {2} damage!";
    public const string WHIFF_CAST = "* {0} attacks {1}! ...But it had no effect!";
    public const string FAIL_CAST = "* {0} attacks {1}! ...But it missed!";
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>();
    public const int SP_GAIN = 1;
    int amount;

    public Attack(Character caster) : base(caster, NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) {
    }

    public override void calculateSuccessRate() {
        successRate = .8;
    }

    public override void onSuccess() {
        amount = caster.getAttribute(AttributeType.STRENGTH).getFalse();
        targets[0].getResource(ResourceType.HEALTH).subtractFalse(amount);
        if (caster.addToResource(ResourceType.SKILL, false, SP_GAIN)) {
            setCastText(string.Format(SUCCESS_CAST, caster.getName(), targets[0].getName(), amount));
        } else {
            setCastText(string.Format(WHIFF_CAST, caster.getName(), targets[0].getName(), amount));
        }
    }

    public override void onFailure() {
        setCastText(string.Format(FAIL_CAST, caster.getName(), targets[0].getName()));
    }

    public override void undo() {
        targets[0].getResource(ResourceType.HEALTH).addFalse(amount);
    }
}
