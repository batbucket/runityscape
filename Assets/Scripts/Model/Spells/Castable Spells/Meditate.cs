using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Meditate : Spell {
    public const string NAME = "Meditate";
    public const SpellType SPELL_TYPE = SpellType.BOOST;
    public const TargetType TARGET_TYPE = TargetType.SELF;
    public const string CAST_TEXT = "* {0} calms their mind.\n{0} restored {1} HP!";
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>() {
        {ResourceType.CHARGE, 100 },
        {ResourceType.SKILL, 3 }
    };

    int amount;

    public Meditate() : base(NAME, SPELL_TYPE, TARGET_TYPE, CAST_TEXT, COSTS) {
    }

    //heals for 30% casterMaxHP + casterIntelligence
    protected override void cast(Game game) {
        Resource casterHealth = caster.getResource(ResourceType.HEALTH);
        Attribute casterIntel = caster.getAttribute(AttributeType.INTELLIGENCE);
        amount = (int)(casterHealth.getTrue() * .3) + casterIntel.getFalse();
        casterHealth.addFalse(amount);
        game.postText(string.Format(CAST_TEXT, caster.getName(), amount));
    }

    public override void undo(Game game) {
        Resource casterHealth = caster.getResource(ResourceType.HEALTH);
        casterHealth.subtractFalse(amount);
    }
}
