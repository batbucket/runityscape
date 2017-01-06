using Scripts.Model.Characters;
using Scripts.Model.Spells;
using Scripts.View.Effects;
using System;
using System.Collections.Generic;

public class Critical : Result {

    public Critical(Func<Character, Character, bool> isState = null,
              Func<Character, Character, float> duration = null,
              Func<Character, Character, float> timePerTick = null,
              Func<Character, Character, bool> isIndefinite = null,
              Action<Spell> react = null,
              Action<Spell> witness = null,
              Func<Character, Character, Calculation> calculation = null,
              Action<Character, Character, Calculation> perform = null,
              Action<Character, Character> onStart = null,
              Action<Character, Character> onEnd = null,
              Func<Character, Character, Calculation, string> createText = null,
              Func<Character, Character, Calculation, string> sound = null,
              Func<Character, Character, Calculation, IList<CharacterEffect>> sfx = null)
        : base(isState, duration, timePerTick, react, witness, isIndefinite, calculation, perform, onStart, onEnd, createText, sound, sfx) {
    }
}