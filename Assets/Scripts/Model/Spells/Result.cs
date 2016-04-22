using System;
using System.Collections.Generic;
using UnityEngine;

public class Result {

    SpellComponent _component;
    public SpellComponent Component { get { return _component; } set { _component = value; } }

    public enum Type {
        UNDEFINED, MISS, CRITICAL, HIT
    }

    public Type type { get; set; }
    Func<Character, Character, bool> isState;
    Func<Character, Character, Calculation> calculation;
    Action<Character, Character, Calculation> perform;
    Func<Character, Character, Calculation, string> createText;
    Action<Character, Character, Calculation> sfx;

    public Result(Func<Character, Character, bool> isState = null,
              Func<Character, Character, Calculation> calculation = null,
              Action<Character, Character, Calculation> perform = null,
              Func<Character, Character, Calculation, string> createText = null,
              Action<Character, Character, Calculation> sfx = null) {
        this.isState = isState ?? ((c, t) => { return false; });
        this.calculation = calculation ?? ((c, t) => { return new Calculation(); });
        this.perform = perform ?? NumericPerform;
        this.createText = createText ?? ((c, t, calc) => { return ""; });
        this.sfx = sfx ?? ((c, t, calc) => { });
    }

    public bool IsState() {
        return isState(Component.Spell.Caster, Component.Spell.Target);
    }

    public Calculation Calculation() {
        return calculation(Component.Spell.Caster, Component.Spell.Target);
    }

    public void Effect(Calculation calculation) {
        perform(Component.Spell.Caster, Component.Spell.Target, calculation);
    }

    public void SFX(Calculation calculation) {
        sfx(Component.Spell.Caster, Component.Spell.Target, calculation);
    }

    public string CreateText(Calculation calculation) {
        return createText(Component.Spell.Caster, Component.Spell.Target, calculation);
    }

    public static void NumericPerform(Character caster, Character target, Calculation calculation) {
        foreach (KeyValuePair<AttributeType, PairedInt> pair in calculation.CasterAttributes) {
            caster.AddToAttribute(pair.Key, true, pair.Value.True, true);
            caster.AddToAttribute(pair.Key, false, pair.Value.False, true);
        }
        foreach (KeyValuePair<AttributeType, PairedInt> pair in calculation.TargetAttributes) {
            target.AddToAttribute(pair.Key, true, pair.Value.True, true);
            target.AddToAttribute(pair.Key, false, pair.Value.False, true);
        }
        foreach (KeyValuePair<ResourceType, PairedInt> pair in calculation.CasterResources) {
            caster.AddToResource(pair.Key, true, pair.Value.True, true);
            caster.AddToResource(pair.Key, false, pair.Value.False, true);
        }
        foreach (KeyValuePair<ResourceType, PairedInt> pair in calculation.TargetResources) {
            target.AddToResource(pair.Key, true, pair.Value.True, true);
            target.AddToResource(pair.Key, false, pair.Value.False, true);
        }
    }
}
