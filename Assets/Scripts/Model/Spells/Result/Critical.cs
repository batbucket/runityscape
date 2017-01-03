using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.View.Effects;
using System;
using System.Collections.Generic;

public class Critical : Result {

    public Critical(Func<Character, Character, Spell, bool> isState = null,
              Func<Character, Character, Spell, float> duration = null,
              Func<Character, Character, Spell, float> timePerTick = null,
              Func<Character, Character, Spell, bool> isIndefinite = null,
              Action<Spell> react = null,
              Action<Spell> witness = null,
              Func<Character, Character, Spell, Calculation> calculation = null,
              Action<Character, Character, Calculation, Spell> perform = null,
              Action<Character, Character, Spell> onStart = null,
              Action<Character, Character, Spell> onEnd = null,
              Func<Character, Character, Calculation, Spell, string> createText = null,
              Func<Character, Character, Calculation, Spell, string> sound = null,
              Func<Character, Character, Calculation, Spell, IList<CharacterEffect>> sfx = null)
        : base(isState, duration, timePerTick, react, witness, isIndefinite, calculation, perform, onStart, onEnd, createText, sound, sfx) {
    }
}