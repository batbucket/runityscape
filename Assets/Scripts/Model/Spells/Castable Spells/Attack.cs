using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Attack : Spell {
    public const string NAME = "Attack";
    public static readonly string DESCRIPTION = string.Format("Attack a single enemy for {0} damage.", Util.Color(Strength.SHORT_NAME, Strength.ASSOCIATED_COLOR));
    public const SpellType SPELL_TYPE = SpellType.OFFENSE;
    public const SpellTarget TARGET_TYPE = SpellTarget.SINGLE_ENEMY;
    public const string SUCCESS_CAST = "* {0} attacks {1} for {2} damage!";
    public const string NO_DAMAGE_CAST = "* {0} attacks {1}! ...But it had no effect!";
    public const string MISS_CAST = "* {0} attacks {1}! ...But it missed!";
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>();
    public const int SP_GAIN = 1;

    public Attack() : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) {
    }

    public override double CalculateHitRate(Character caster, Character target) {
        return .8;
    }

    public override int CalculateAmount(Character caster, Character target) {
        return Amount = caster.GetAttributeCount(AttributeType.STRENGTH, false);
    }

    protected override void OnHit(Character caster, Character target) {
        CalculateAmount(caster, target);
        target.AddToResource(ResourceType.HEALTH, false, -Amount);
        caster.AddToResource(ResourceType.SKILL, false, SP_GAIN);

        if (Amount > 0) {
            OnSuccess(caster, target);
        } else {
            OnFailure(caster, target);
        }
    }

    protected override void OnSuccess(Character caster, Character target) {
        CastText = string.Format(SUCCESS_CAST, caster.Name, target.Name, Amount);
        EffectsManager.CreateBloodsplat(target);
        SoundView.Instance.Play("Sounds/Attack_0");
        EffectsManager.CreateHitsplat(Amount, Health.UNDER_COLOR, target);
        EffectsManager.Instance.RedFadeEffect(target);
    }

    protected override void OnFailure(Character caster, Character target) {
        CastText = string.Format(NO_DAMAGE_CAST, caster.Name, target.Name, Amount);
        EffectsManager.CreateHitsplat(Amount, Color.grey, target);
    }

    protected override void OnMiss(Character caster, Character target) {
        CastText = string.Format(MISS_CAST, caster.Name, target.Name);
    }

    public override void Undo() {
        if (Result == SpellResult.HIT) {
            Target.AddToResource(ResourceType.HEALTH, false, Amount);
        }
    }
}
