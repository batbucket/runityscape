﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SummonTent : SpellFactory {
    private IList<Mimic> tents;

    public SummonTent(IList<Mimic> tents) : base("SummonTent", "Summon tentacles", SpellType.OFFENSE, TargetType.SELF) {
        this.tents = tents;
    }

    public override Hit CreateHit() {
        return new Hit(
            isState: (c, t, o) => true,
            calculation: (c, t, o) => {
                return new Calculation(
                    casterResources: new Dictionary<ResourceType, PairedInt>() {
                        { ResourceType.CORRUPTION, new PairedInt(0, -c.GetResourceCount(ResourceType.CORRUPTION, false)) }
                    }
                    );
            },
            perform: (c, t, calc, o) => {
                for (int i = 0; i < tents.Count; i++) {
                    Mimic s = tents[i].Summon();
                    Game.Instance.CurrentPage.AddCharacters(c.Side, s);
                }
                Result.NumericPerform(c, t, calc);
            },
            createText: (c, t, calc, o) => string.Format("Tentacles erupt from the ground on {0}'s side!", c.DisplayName)
            );
    }
}
