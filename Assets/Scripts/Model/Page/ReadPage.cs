using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

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
        Process[] processes = null,
        string musicLoc = null
        )
        : base(text, tooltip, location, hasInputField, mainCharacter, left, right, onFirstEnter, onEnter, onFirstExit, onExit, onTick, processes, musicLoc) {
    }

    public override void Tick() {
        base.Tick();
        GetAll().Where(c => c.HasResource(ResourceType.CHARGE)).ToList().ForEach(c => c.Resources[ResourceType.CHARGE].IsVisible = false);
    }
}
