using System;
using System.Collections.Generic;

public abstract class CounterSpellFactory : SpellFactory {
    public CounterSpellFactory(SpellType spellType,
                               TargetType targetType)
                               : base("", "", spellType, targetType, new Dictionary<ResourceType, int>()) { }

    protected override void ConsumeResources(Character caster) {
        foreach (KeyValuePair<ResourceType, int> resourceCost in _costs) {
            caster.AddToResource(resourceCost.Key, false, -resourceCost.Value);
        }
    }

    public override bool IsCastable(Character caster, Character target = null) {
        if (target != null && !target.IsTargetable) {
            return false;
        }
        if (!IsEnabled) {
            return false;
        }
        foreach (KeyValuePair<ResourceType, int> resourceCost in _costs) {
            if (!caster.Resources.ContainsKey(resourceCost.Key) || caster.GetResourceCount(resourceCost.Key, false) < resourceCost.Value) {
                return false;
            }
        }
        return true;
    }
}
