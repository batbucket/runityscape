using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Critical : Result {
    public Critical(Func<Character, Character, SpellDetails, bool> isState = null,
              Func<Character, Character, SpellDetails, float> duration = null,
              Func<Character, Character, SpellDetails, float> timePerTick = null,
              Func<Character, Character, SpellDetails, bool> isIndefinite = null,
              Func<Character, Character, SpellDetails, Calculation> calculation = null,
              Action<Character, Character, Calculation, SpellDetails> perform = null,
              Action<Character, Character, SpellDetails> onStart = null,
              Action<Character, Character, SpellDetails> onEnd = null,
              Func<Character, Character, Calculation, SpellDetails, string> createText = null,
              Func<Character, Character, Calculation, SpellDetails, string> sound = null,
              Func<Character, Character, Calculation, SpellDetails, IList<CharacterEffect>> sfx = null)
        : base(isState, duration, timePerTick, isIndefinite, calculation, perform, onStart, onEnd, createText, sound, sfx) {

    }
}
