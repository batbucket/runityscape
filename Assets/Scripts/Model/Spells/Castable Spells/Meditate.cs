using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Meditate : Spell {
    public const string NAME = "Meditate";
    public const string DESCRIPTION = "Heal yourself.";
    public const SpellType SPELL_TYPE = SpellType.BOOST;
    public const SpellTarget TARGET_TYPE = SpellTarget.SELF;
    public static readonly string[] CAST_TEXT = new string[] { "* {0} calms their mind, restoring {1} HP!" };
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>() {
        {ResourceType.SKILL, 3 }
    };

    int amount;

    public Meditate() : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) {
    }

    public override double CalculateHitRate(Character caster, Character target) {
        return 1;
    }

    public override int CalculateDamage(Character caster, Character target) {
        return Damage = 0;
    }

    protected override void OnSuccess(Character caster, Character target) {
        Resource casterHealth = caster.GetResource(ResourceType.HEALTH);
        Attribute casterIntel = caster.GetAttribute(AttributeType.INTELLIGENCE);
        amount = (int)(casterHealth.True * .3) + casterIntel.False;
        casterHealth.False += amount;
        CastText = string.Format(CAST_TEXT[0], caster.Name, amount);
    }

    protected override void OnFailure(Character caster, Character target) {
        throw new NotImplementedException();
    }

    public override void Undo() {
        Resource casterHealth = Caster.GetResource(ResourceType.HEALTH);
        casterHealth.False -= amount;
    }
}
