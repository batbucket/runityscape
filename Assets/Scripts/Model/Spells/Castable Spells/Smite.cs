using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Smite : SpellFactory {
    const string NAME = "Smite";
    static readonly string DESCRIPTION = string.Format("Deal <color=red>{0}</color> unresistable damage.", DAMAGE);
    static readonly string TEXT = "{0} invokes the wrath of the heavens on {1} for <color=red>{2}</color> damage!";
    const SpellType SPELL_TYPE = SpellType.OFFENSE;
    const TargetType TARGET_TYPE = TargetType.SINGLE_ENEMY;
    static readonly IDictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>() {
        { ResourceType.SKILL, 2 }
    };
    public const int DAMAGE = 10;

    public Smite() : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) { }

    public override Hit CreateHit() {
        return new Hit(
            isState: (c, t, o) => true,
            calculation: (c, t, o) => {
                return new Calculation(
                   targetResources: new Dictionary<ResourceType, PairedInt>() {
                       { ResourceType.HEALTH, new PairedInt(0, -DAMAGE) }
                    }
                );
            },
            createText: (c, t, calc, o) => {
                return string.Format(TEXT, c.DisplayName, t.DisplayName, (int)-calc.TargetResources[ResourceType.HEALTH].False);
            },
            sound: (c, t, calc, o) => {
                return "Boom_4";
            },
            sfx: (c, t, calc, o) => {
                return Result.AppendToStandard(c, t, calc, new LightningEffect(t.Presenter.PortraitView));
            }
        );
    }
}