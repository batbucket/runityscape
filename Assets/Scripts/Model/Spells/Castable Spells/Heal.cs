﻿using System.Collections.Generic;

public class Heal : SpellFactory {
    public Heal() : base(SpellType.BOOST, TargetType.SINGLE_ALLY, "Heal", "Heal a target.") { }

    public override Hit CreateHit() {
        return new Hit(
            isState: (c, t, o) => true,
            calculation: (c, t, o) => new Calculation(targetResources: new Dictionary<ResourceType, PairedInt>() { { ResourceType.HEALTH, new PairedInt(0, 2) } }),
            sound: (c, t, calc, o) => "Zip_0",
            createText: (c, t, calc, o) =>
                string.Format("{0} heals {1} for {2} life.",
                    c.DisplayName,
                    c != t ? t.DisplayName : "themselves",
                    Util.Color(calc.TargetResources[ResourceType.HEALTH].False + "", ResourceType.HEALTH.FillColor))
            );
    }

    protected override bool Castable(Character caster, Character target) {
        return target.GetResourceCount(ResourceType.HEALTH, false) < target.GetResourceCount(ResourceType.HEALTH, true);
    }
}