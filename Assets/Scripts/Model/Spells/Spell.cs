using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Spell {
    readonly string name;
    readonly SpellType spellType;
    readonly TargetType targetType;
    readonly string castText;
    Dictionary<ResourceType, int> costs;

    protected Character caster;
    protected List<Character> targets;

    public Spell(string name, SpellType spellType, TargetType targetType, string castText, Dictionary<ResourceType, int> costs) {
        this.name = name;
        this.spellType = spellType;
        this.targetType = targetType;
        this.castText = castText;
        this.costs = costs;
        this.targets = new List<Character>();
    }

    public Spell initialize(Character caster, params Character[] targets) {
        this.caster = caster;
        this.targets.AddRange(targets);
        return this;
    }

    public string getName() {
        return name;
    }

    public string getCosts() {
        string s = " - ";
        List<string> elements = new List<string>();
        foreach (KeyValuePair<ResourceType, int> entry in costs) {
            if (entry.Key != ResourceType.CHARGE) {
                Color c = ResourceFactory.createResource(entry.Key, 0).getOverColor();
                int cost = entry.Value;
                elements.Add(Util.color("" + cost, c));
            }
        }
        s += string.Join("/", elements.ToArray());
        return elements.Count == 0 ? "" : s;
    }

    public string getNameAndCosts() {
        string s = name + " - ";
        List<string> elements = new List<string>();
        foreach (KeyValuePair<ResourceType, int> entry in costs) {
            if (entry.Key != ResourceType.CHARGE) {
                Color c = ResourceFactory.createResource(entry.Key, 0).getOverColor();
                int cost = entry.Value;
                elements.Add(Util.color("" + cost, c));
            }
        }
        s += string.Join("/", elements.ToArray());

        return elements.Count == 0 ? name : s;
    }

    public SpellType getSpellType() {
        return spellType;
    }

    public TargetType getTargetType() {
        return targetType;
    }

    public bool canCast(Character character) {
        foreach (KeyValuePair<ResourceType, int> resourceCost in costs) {
            if (character.getResource(resourceCost.Key) == null || character.getResource(resourceCost.Key).getFalse() < resourceCost.Value) {
                return false;
            }
        }
        return true;
    }

    public void consumeResources(Character character) {
        if (canCast(character)) {
            foreach (KeyValuePair<ResourceType, int> resourceCost in costs) {
                character.getResource(resourceCost.Key).subtractFalse(resourceCost.Value);
            }
        }
    }

    public void reactions(Game game) {
        foreach(Character character in targets) {
            character.react(this, game);
        }
    }

    public void tryCast(Game game) {
        if (canCast(caster)) {
            cast(game);
            consumeResources(caster);
            reactions(game);
        }
    }
    protected abstract void cast(Game game);
    public abstract void undo(Game game);
}
