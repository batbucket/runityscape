using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Spells always consume 100% of the user's charge bar
 * which is not considered to be a cost as it varies
 */
public abstract class Spell {
    readonly string name;
    readonly SpellType spellType;
    readonly TargetType targetType;
    Dictionary<ResourceType, int> costs;

    protected Character caster;
    protected List<Character> targets;

    protected double successRate;
    protected bool hit;

    public Spell(Character caster, string name, SpellType spellType, TargetType targetType, Dictionary<ResourceType, int> costs) {
        this.caster = caster;
        this.name = name;
        this.spellType = spellType;
        this.targetType = targetType;
        this.costs = costs;
        this.targets = new List<Character>();
    }

    public Spell setTargets(params Character[] targets) {
        this.targets.AddRange(targets);
        return this;
    }

    public string getName() {
        return name;
    }

    public string getNameAndCosts() {
        string s = (canCast() ? name : "<color=red>" + name + "</color>") + " - ";
        List<string> elements = new List<string>();
        foreach (KeyValuePair<ResourceType, int> entry in costs) {
            if (entry.Key != ResourceType.CHARGE) {
                Color c = ResourceFactory.createResource(entry.Key, 0).getOverColor();
                int cost = entry.Value;
                elements.Add(Util.color("" + cost, c));
            }
        }
        s += string.Join("/", elements.ToArray());
        return elements.Count == 0 ? (canCast() ? name : "<color=red>" + name + "</color>") : s;
    }

    public SpellType getSpellType() {
        return spellType;
    }

    public TargetType getTargetType() {
        return targetType;
    }

    public bool canCast() {
        foreach (KeyValuePair<ResourceType, int> resourceCost in costs) {
            if (caster.getResource(resourceCost.Key) == null || caster.getResource(resourceCost.Key).getFalse() < resourceCost.Value) {
                return false;
            }
        }
        return caster.getResource(ResourceType.CHARGE).isMaxed();
    }

    public void consumeResources() {
        foreach (KeyValuePair<ResourceType, int> resourceCost in costs) {
            caster.getResource(resourceCost.Key).subtractFalse(resourceCost.Value);
        }
        caster.getResource(ResourceType.CHARGE).clearFalse();
    }

    public void reactions(Game game) {
        foreach(Character character in targets) {
            character.react(this, game);
        }
    }

    public bool tryCast(Game game) {
        calculateSuccessRate();
        if (canCast()) {
            cast(game);
            consumeResources();
            reactions(game);
            return true;
        }
        return false;
    }

    protected void cast(Game game) {
        if (hit = (Util.chance(successRate))) {
            onSuccess(game);
        } else {
            onFailure(game);
        }
    }

    public virtual void calculateSuccessRate() {
        successRate = 1;
    }

    public abstract void onSuccess(Game game);
    public abstract void onFailure(Game game);
    public abstract void undo(Game game);
    public bool isHit() {
        return hit;
    }
}
