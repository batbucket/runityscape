using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ReadPage : Page {

    public ReadPage(
        string text = "",
        string tooltip = "",
        Character mainCharacter = null,
        Character[] left = null,
        Character[] right = null,
        Action onFirstEnter = null,
        Action onEnter = null,
        Action onFirstExit = null,
        Action onExit = null,
        Action onTick = null,
        Process[] processes = null
        )
        : base(text, tooltip, mainCharacter, left, right, onFirstEnter, onEnter, onFirstExit, onExit, onTick, processes) {
    }
}
