using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ReadPage : Page {

    public ReadPage(string text = "", string tooltip = "", Character mainCharacter = null, List<Character> left = null, List<Character> right = null,
        Action onFirstEnter = null, Action onEnter = null, Action onFirstExit = null, Action onExit = null, Action onTick = null, params Process[] processes)
            : base(text, tooltip, mainCharacter, left, right, onFirstEnter, onEnter, onFirstExit, onExit, onTick, processes) {
    }
}
