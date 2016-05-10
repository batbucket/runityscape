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
    Func<Character, Character, SpellDetails, bool> _isState;
    Func<Character, Character, SpellDetails, Calculation> _calculation;
    Action<Character, Character, Calculation, SpellDetails> _perform;
    Func<Character, Character, Calculation, SpellDetails, string> _createText;
    Action<Character, Character, Calculation, SpellDetails> _sfx;

    public Func<Character, Character, SpellDetails, bool> IsStateFunc { set { _isState = value; } }
    public Func<Character, Character, SpellDetails, Calculation> CalculationFunc { set { _calculation = value; } }
    public Action<Character, Character, Calculation, SpellDetails> PerformFunc { set { _perform = value; } }
    public Func<Character, Character, Calculation, SpellDetails, string> CreateTextFunc { set { _createText = value; } }
    public Action<Character, Character, Calculation, SpellDetails> SFXFunc { set { _sfx = value; } }

    public Result(Func<Character, Character, SpellDetails, bool> isState = null,
              Func<Character, Character, SpellDetails, Calculation> calculation = null,
              Action<Character, Character, Calculation, SpellDetails> perform = null,
              Func<Character, Character, Calculation, SpellDetails, string> createText = null,
              Action<Character, Character, Calculation, SpellDetails> sfx = null) {
        this._isState = isState ?? ((c, t, o) => { return false; });
        this._calculation = calculation ?? ((c, t, o) => { return new Calculation(); });
        this._perform = perform ?? ((c, t, calc, o) => { NumericPerform(c, t, calc); });
        this._createText = createText ?? ((c, t, calc, o) => { return ""; });
        this._sfx = sfx ?? ((c, t, calc, o) => { });
    }

    public bool IsState() {
        return _isState(Component.Spell.Caster, Component.Spell.Target, Component.Spell.Other);
    }

    public Calculation Calculation() {
        Calculation c = _calculation(Component.Spell.Caster, Component.Spell.Target, Component.Spell.Other);
        c.Result = this;
        return c;
    }

    public void Effect(Calculation calculation) {
        _perform(Component.Spell.Caster, Component.Spell.Target, calculation, Component.Spell.Other);
    }

    public void SFX(Calculation calculation) {
        _sfx(Component.Spell.Caster, Component.Spell.Target, calculation, Component.Spell.Other);
    }

    public string CreateText(Calculation calculation) {
        return _createText(Component.Spell.Caster, Component.Spell.Target, calculation, Component.Spell.Other);
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
