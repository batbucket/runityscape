using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Miss : Result {
    public Miss(Func<Character, Character, SpellDetails, bool> isState = null,
              Func<Character, Character, SpellDetails, Calculation> calculation = null,
              Action<Character, Character, Calculation, SpellDetails> perform = null,
              Func<Character, Character, Calculation, SpellDetails, string> createText = null,
              Func<Character, Character, Calculation, SpellDetails, string> sound = null,
              Func<Character, Character, Calculation, SpellDetails, IList<CharacterEffect>> sfx = null)
        : base(null, calculation, perform, createText, sound, sfx) {

    }
}
