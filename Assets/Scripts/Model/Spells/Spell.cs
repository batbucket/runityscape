using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
    protected SpellResult result;

    protected string castText;

    public Spell(Character caster, string name, SpellType spellType, TargetType targetType, Dictionary<ResourceType, int> costs) {
        this.caster = caster;
        this.name = name;
        this.spellType = spellType;
        this.targetType = targetType;
        this.costs = costs;
        this.targets = new List<Character>();
    }

    public Spell(Character caster, string name, SpellType spellType, TargetType targetType)
    {
        this.caster = caster;
        this.name = name;
        this.spellType = spellType;
        this.targetType = targetType;
        this.costs = new Dictionary<ResourceType, int>();
        this.targets = new List<Character>();
    }

    public void setTargets(List<Character> targets) {
        this.targets = targets;
    }

    public void setTarget(Character target) {
        this.targets.Clear();
        this.targets.Add(target);
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

    public virtual bool canCast() {
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

    public void tryCast() {
        if (canCast()) {
            cast();
            consumeResources();
        } else {
            result = SpellResult.CANT_CAST;
        }
    }

    protected void cast() {
        calculateSuccessRate();
        if (Util.chance(successRate)) {
            onSuccess();
            result = SpellResult.HIT;
        } else {
            onFailure();
            result = SpellResult.MISS;
        }
    }

    public virtual void calculateSuccessRate() {
        successRate = 1;
    }

    public abstract void onSuccess();
    public abstract void onFailure();
    public abstract void undo();
    public SpellResult getResult() {
        return result;
    }

    public string getCastText() {
        return castText;
    }

    protected void setCastText(string s) {
        castText = s;
    }
}
