using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Meditate : SpellFactory {
    public const string NAME = "Meditate";
    public const string DESCRIPTION = "Heal yourself.";
    public const SpellType SPELL_TYPE = SpellType.BOOST;
    public const TargetType TARGET_TYPE = TargetType.SELF;
    public static readonly string CAST_TEXT = "* {0} calms their mind, restoring {1} HP!";
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>() {
        {ResourceType.SKILL, 3 }
    };

    public Meditate() : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) { }

    protected override IDictionary<string, SpellComponent> CreateComponents(Character caster, Character target, Spell spell) {
        return new Dictionary<string, SpellComponent>() {
            { SpellFactory.PRIMARY, new SpellComponent(
                hit: new Result(
                    isState: (c, t) => {
                        return true;
                    },
                    calculation: (c, t) => {
                        return new Calculation(
                            targetResources: new Dictionary<ResourceType, PairedInt>() {
                                { ResourceType.HEALTH, new PairedInt(0, 3) } //TODO change back to t.GetResourceCount(ResourceType.HEALTH, true) / 2
                            });
                    },
                    //Implicit perform
                    createText: (c, t, calc) => {
                        return string.Format(CAST_TEXT, t, calc.TargetResources[ResourceType.HEALTH].False);
                    },
                    sfx: (c, t, calc) => {
                        Game.Instance.Sound.Play("Sounds/Zip_0");
                    }
                )) }
        };
    }
}
