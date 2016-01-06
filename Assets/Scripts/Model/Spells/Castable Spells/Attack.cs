using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Attack : Spell {
    public const string NAME = "Attack";
    public const SpellType SPELL_TYPE = SpellType.OFFENSE;
    public const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
    public const string SUCCESS_CAST = "* {0} attacks {1} for {2} damage!";
    public const string FAIL_CAST = "* {0} attacks {1}! ...But it missed!";
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>();
    public const int SP_GAIN = 1;
    int amount;

    public Attack(Character caster) : base(caster, NAME, SPELL_TYPE, TARGET_TYPE, COSTS) {
    }

    public override void calculateSuccessRate() {
        successRate = .8;
    }

    public override void onSuccess(Game game) {
        amount = caster.getAttribute(AttributeType.STRENGTH).getFalse();
        targets[0].getResource(ResourceType.HEALTH).subtractFalse(amount);
        caster.addToResource(ResourceType.SKILL, false, SP_GAIN);
        game.postText(string.Format(SUCCESS_CAST, caster.getName(), targets[0].getName(), amount));
    }

    public override void onFailure(Game game) {
        game.postText(string.Format(FAIL_CAST, caster.getName(), targets[0].getName()));
    }

    public override void undo(Game game) {
        targets[0].getResource(ResourceType.HEALTH).addFalse(amount);
    }
}
