using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Attack : SpellFactory {
    public const string NAME = "Attack";
    public static readonly string DESCRIPTION = string.Format("Attack a single enemy for {0} damage.", Util.Color(AttributeType.STRENGTH.ShortName, AttributeType.STRENGTH.Color));
    public const SpellType SPELL_TYPE = SpellType.OFFENSE;
    public const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
    public const string SUCCESS_TEXT = "* {0} attacks {1} for {2} damage!";
    public const string CRITICAL_TEXT = "* {0} critically strikes {1} for {2} damage!";
    public const string MISS_TEXT = "* {0} attacks {1}... But it missed!";
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>();
    public const int SP_GAIN = 1;

    public Attack() : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) { }

    public override bool IsHit(Spell spell) {
        return Util.Chance(.8);
    }

    protected override void OnHitCalculation(Spell spell) {
        spell.Resources[ResourceType.HEALTH].False = -UnityEngine.Random.Range(spell.Caster.GetAttributeCount(AttributeType.INTELLIGENCE, false), spell.Caster.GetAttributeCount(AttributeType.STRENGTH, false));
    }

    protected override void OnHitSFX(Spell spell) {
        Game.Instance.Sound.Play("Sounds/Attack_0");
    }

    protected override string OnHitText(Spell spell) {
        return string.Format(SUCCESS_TEXT, spell.Caster, spell.Target, -spell.Resources[ResourceType.HEALTH].False);
    }

    protected override string OnMissText(Spell spell) {
        return string.Format(MISS_TEXT, spell.Caster, spell.Target);
    }

    public override bool IsCritical(Spell spell) {
        return Util.Chance(.2);
    }

    protected override void OnCriticalCalculation(Spell spell) {
        spell.Resources[ResourceType.HEALTH].False = -UnityEngine.Random.Range(spell.Caster.GetAttributeCount(AttributeType.INTELLIGENCE, false), spell.Caster.GetAttributeCount(AttributeType.STRENGTH, false)) * 2;
    }

    protected override string OnCriticalText(Spell spell) {
        return string.Format(CRITICAL_TEXT, spell.Caster, spell.Target, -spell.Resources[ResourceType.HEALTH].False);
    }

    protected override void OnCriticalSFX(Spell spell) {
        Game.Instance.Sound.Play("Sounds/Attack_0");
        Game.Instance.Effect.CreateBloodsplat(spell.Target);
    }

    protected override void OnOnce(Character caster) {
        caster.AddToResource(ResourceType.SKILL, false, 1, true);
    }
}
