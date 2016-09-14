using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Meditate : SpellFactory {
    public const string NAME = "Meditate";
    public const string DESCRIPTION = "Heal yourself for 50% + <color=blue>INT * 2</color> your maximum <color=lime>life</color>.";
    public const SpellType SPELL_TYPE = SpellType.BOOST;
    public const TargetType TARGET_TYPE = TargetType.SELF;
    public static readonly string CAST_TEXT = "{0} calms their mind, restoring {1} <color=lime>life</color>!";
    public static readonly Dictionary<ResourceType, int> COSTS = new Dictionary<ResourceType, int>() {
        {ResourceType.SKILL, 3 }
    };

    public Meditate() : base(NAME, DESCRIPTION, SPELL_TYPE, TARGET_TYPE, COSTS) { }

    public override Hit CreateHit() {
        return new Hit(
            isState: (c, t, o) => {
                return true;
            },
            calculation: (c, t, o) => {
                return new Calculation(
                    targetResources: new Dictionary<ResourceType, PairedInt>() {
                    { ResourceType.HEALTH, new PairedInt(0, t.GetResourceCount(ResourceType.HEALTH, true) / 2 + t.GetAttributeCount(AttributeType.INTELLIGENCE, false) * 2) }
                    });
            },
            createText: (c, t, calc, o) => {
                return string.Format(CAST_TEXT, t.DisplayName, calc.TargetResources[ResourceType.HEALTH].False);
            },
            sound: (c, t, calc, o) => {
                return "Zip_0";
            }
        );
    }
}
