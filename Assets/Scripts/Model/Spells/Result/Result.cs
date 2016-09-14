using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Result {

    Spell _spell;
    public Spell Spell {
        set {
            Util.Assert(_spell == null, "Can only set Spell once!");
            _spell = value;
        }

        get {
            return _spell;
        }
    }

    public Type type { get; set; }
    Func<Character, Character, SpellDetails, bool> _isState;
    Func<Character, Character, SpellDetails, Calculation> _calculation;
    Action<Character, Character, Calculation, SpellDetails> _perform;
    Func<Character, Character, Calculation, SpellDetails, string> _createText;
    Func<Character, Character, Calculation, SpellDetails, string> _sound;
    Func<Character, Character, Calculation, SpellDetails, IList<CharacterEffect>> _sfx;

    public Func<Character, Character, SpellDetails, bool> IsStateFunc { set { _isState = value; } }
    public Func<Character, Character, SpellDetails, Calculation> CalculationFunc { set { _calculation = value; } }
    public Action<Character, Character, Calculation, SpellDetails> PerformFunc { set { _perform = value; } }
    public Func<Character, Character, Calculation, SpellDetails, string> CreateTextFunc { set { _createText = value; } }
    public Func<Character, Character, Calculation, SpellDetails, string> SoundFunc { set { _sound = value; } }
    public Func<Character, Character, Calculation, SpellDetails, IList<CharacterEffect>> SFXFunc { set { _sfx = value; } }

    public Result(Func<Character, Character, SpellDetails, bool> isState = null,
              Func<Character, Character, SpellDetails, Calculation> calculation = null,
              Action<Character, Character, Calculation, SpellDetails> perform = null,
              Func<Character, Character, Calculation, SpellDetails, string> createText = null,
              Func<Character, Character, Calculation, SpellDetails, string> sound = null,
              Func<Character, Character, Calculation, SpellDetails, IList<CharacterEffect>> sfx = null) {
        this._isState = isState ?? ((c, t, o) => { return false; });
        this._calculation = calculation ?? ((c, t, o) => { return new Calculation(); });
        this._perform = perform ?? ((c, t, calc, o) => { NumericPerform(c, t, calc); });
        this._createText = createText ?? ((c, t, calc, o) => { return ""; });
        this._sound = sound ?? ((c, t, calc, o) => { return ""; });
        this._sfx = sfx ?? ((c, t, calc, o) => { return new CharacterEffect[0]; });
    }

    public bool IsState() {
        return _isState(_spell.Caster, _spell.Target, _spell.Other);
    }

    public Calculation Calculation() {
        Calculation c = _calculation(_spell.Caster, _spell.Target, _spell.Other);
        return c;
    }

    public void Effect(Calculation calculation) {
        _perform(_spell.Caster, _spell.Target, calculation, _spell.Other);
    }

    public IList<CharacterEffect> SFX(Calculation calculation) {
        return _sfx(_spell.Caster, _spell.Target, calculation, _spell.Other);
    }

    public string CreateText(Calculation calculation) {
        return _createText(_spell.Caster, _spell.Target, calculation, _spell.Other);
    }

    public string Sound(Calculation calculation) {
        return _sound(_spell.Caster, _spell.Target, calculation, _spell.Other);
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

    public static void NumericUndo(Character caster, Character target, Calculation calculation) {
        foreach (KeyValuePair<AttributeType, PairedInt> pair in calculation.CasterAttributes) {
            caster.AddToAttribute(pair.Key, true, -pair.Value.True, true);
            caster.AddToAttribute(pair.Key, false, -pair.Value.False, true);
        }
        foreach (KeyValuePair<AttributeType, PairedInt> pair in calculation.TargetAttributes) {
            target.AddToAttribute(pair.Key, true, -pair.Value.True, true);
            target.AddToAttribute(pair.Key, false, -pair.Value.False, true);
        }
        foreach (KeyValuePair<ResourceType, PairedInt> pair in calculation.CasterResources) {
            caster.AddToResource(pair.Key, true, -pair.Value.True, true);
            caster.AddToResource(pair.Key, false, -pair.Value.False, true);
        }
        foreach (KeyValuePair<ResourceType, PairedInt> pair in calculation.TargetResources) {
            target.AddToResource(pair.Key, true, -pair.Value.True, true);
            target.AddToResource(pair.Key, false, -pair.Value.False, true);
        }
    }
}
