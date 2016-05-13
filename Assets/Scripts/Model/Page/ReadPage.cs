using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ReadPage : Page {

    public ReadPage(
        string text = "",
        string tooltip = "",
        string location = "",
        bool hasInputField = false,
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
        : base(text, tooltip, location, hasInputField, mainCharacter, left, right, onFirstEnter, onEnter, onFirstExit, onExit, onTick, processes) {
    }
}
