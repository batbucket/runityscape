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
    public const string FAIL_CAST = "* {0} attacks {1}! ...But it missed!";
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>();
    public const int SP_GAIN = 1;

    public Attack() : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) {
    }

    public override double CalculateHitRate(Character caster, Character target) {
        return .8;
    }

    public override int CalculateDamage(Character caster, Character target) {
        return Damage = caster.GetAttribute(AttributeType.STRENGTH).False;
    }

    protected override void OnSuccess(Character caster, Character target) {
        CalculateDamage(caster, target);
        target.GetResource(ResourceType.HEALTH).False -= Damage;
        caster.AddToResource(ResourceType.SKILL, false, SP_GAIN);
        if (Damage > 0) {
            CastText = string.Format(SUCCESS_CAST, caster.Name, target.Name, Damage);

            //Hitsplat effect
            GameObject hitsplat = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Hitsplat"));
            hitsplat.GetComponent<HitsplatView>().GrowAndFade(Damage + "!", Color.red);
            Util.Parent(hitsplat, target.Presenter.PortraitView.gameObject);
        } else {
            CastText = string.Format(NO_DAMAGE_CAST, caster.Name, target.Name, Damage);
        }
    }

    protected override void OnFailure(Character caster, Character target) {
        CastText = string.Format(FAIL_CAST, caster.Name, target.Name);
    }

    public override void Undo() {
        if (Result == SpellResult.HIT) {
            Target.GetResource(ResourceType.HEALTH).False += Damage;
        }
    }
}
