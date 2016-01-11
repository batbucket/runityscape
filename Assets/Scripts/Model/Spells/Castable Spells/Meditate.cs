using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Meditate : Spell {
    public const string NAME = "Meditate";
    public const string DESCRIPTION = "Heal yourself.";
    public const SpellType SPELL_TYPE = SpellType.BOOST;
    public const TargetType TARGET_TYPE = TargetType.SELF;
    public static readonly string[] CAST_TEXT = new string[] {"* {0} calms their mind, restoring {1} HP!" };
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>() {
        {ResourceType.SKILL, 3 }
    };

    int amount;

    public Meditate(Character caster) : base(caster, NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) {
    }

    public override void undo() {
        Resource casterHealth = caster.getResource(ResourceType.HEALTH);
        casterHealth.subtractFalse(amount);
    }

    public override void onSuccess() {
        Resource casterHealth = caster.getResource(ResourceType.HEALTH);
        Attribute casterIntel = caster.getAttribute(AttributeType.INTELLIGENCE);
        amount = (int)(casterHealth.getTrue() * .3) + casterIntel.getFalse();
        casterHealth.addFalse(amount);
        setCastText(string.Format(CAST_TEXT[0], caster.getName(), amount));
    }

    public override void onFailure() {
        throw new NotImplementedException();
    }
}
