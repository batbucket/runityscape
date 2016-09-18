using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Result {

    public Type Type { get; set; }
    public Func<Character, Character, SpellDetails, bool> IsState;
    public Func<Character, Character, SpellDetails, bool> IsIndefinite;
    public Func<Character, Character, SpellDetails, float> Duration;
    public Func<Character, Character, SpellDetails, float> TimePerTick;
    public Func<Character, Character, SpellDetails, Calculation> Calculation;
    public Action<Character, Character, Calculation, SpellDetails> Perform;
    public Action<Character, Character, SpellDetails> OnStart;
    public Action<Character, Character, SpellDetails> OnEnd;
    public Func<Character, Character, Calculation, SpellDetails, string> CreateText;
    public Func<Character, Character, Calculation, SpellDetails, string> Sound;
    public Func<Character, Character, Calculation, SpellDetails, IList<CharacterEffect>> Sfx;

    public Result(Func<Character, Character, SpellDetails, bool> isState = null,
              Func<Character, Character, SpellDetails, float> duration = null,
              Func<Character, Character, SpellDetails, float> timePerTick = null,
              Func<Character, Character, SpellDetails, bool> isIndefinite = null,
              Func<Character, Character, SpellDetails, Calculation> calculation = null,
              Action<Character, Character, Calculation, SpellDetails> perform = null,
              Action<Character, Character, SpellDetails> onStart = null,
              Action<Character, Character, SpellDetails> onEnd = null,
              Func<Character, Character, Calculation, SpellDetails, string> createText = null,
              Func<Character, Character, Calculation, SpellDetails, string> sound = null,
              Func<Character, Character, Calculation, SpellDetails, IList<CharacterEffect>> sfx = null) {
        this.IsState = isState ?? ((c, t, o) => { return false; });
        this.Duration = duration ?? ((c, t, o) => { return 0; });
        this.TimePerTick = timePerTick ?? ((c, t, o) => { return 0; });
        this.IsIndefinite = isIndefinite ?? ((c, t, o) => { return false; });
        this.Calculation = calculation ?? ((c, t, o) => { return new Calculation(); });
        this.Perform = perform ?? ((c, t, calc, o) => { NumericPerform(c, t, calc); });
        this.OnStart = onStart ?? ((c, t, o) => { });
        this.OnEnd = onStart ?? ((c, t, o) => { });
        this.CreateText = createText ?? ((c, t, calc, o) => { return ""; });
        this.Sound = sound ?? ((c, t, calc, o) => { return ""; });
        this.Sfx = sfx ?? ((c, t, calc, o) => { return new CharacterEffect[0]; });
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
