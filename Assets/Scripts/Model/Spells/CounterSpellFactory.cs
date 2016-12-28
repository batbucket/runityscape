using System;
using System.Collections.Generic;

public abstract class CounterSpellFactory : SpellFactory {
    public CounterSpellFactory(SpellType spellType,
                               TargetType targetType)
                               : base("", "", spellType, targetType) { }

    protected override void ConsumeResources(Character caster) {
        foreach (KeyValuePair<ResourceType, int> resourceCost in costs) {
            caster.AddToResource(resourceCost.Key, false, -resourceCost.Value);
        }
    }
}
