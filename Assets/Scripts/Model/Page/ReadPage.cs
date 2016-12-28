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
        IList<Character> left = null,
        IList<Character> right = null,
        PageActions pageActions = new PageActions() { },
        IList<IButtonable> buttonables = null,
        string musicLoc = null
        )
        : base(text, tooltip, location, hasInputField, mainCharacter, left, right, pageActions.onFirstEnter, pageActions.onEnter, pageActions.onFirstExit, pageActions.onExit, pageActions.onTick, buttonables, musicLoc) {
    }

    public ReadPage(
    Party party,
    string musicLoc,
    string text,
    string tooltip,
    string location,
    IList<IButtonable> buttonables,
    IList<Character> right = null) : this(party, musicLoc, text, tooltip, location, buttonables, new PageActions() { }, right) { }

    public ReadPage(
    Party party,
    string musicLoc,
    string text,
    string tooltip,
    string location,
    IList<IButtonable> buttonables,
    PageActions pageActions,
    IList<Character> right = null) : this(text, tooltip, location, false, party.Main, party.Members, right, pageActions, buttonables, musicLoc) {

        // If the party ever changes, update the side main is on
        OnTickAction += () => {
            MainCharacter = party.Main;
            IList<Character> addSide = (party.Main.Side ? RightCharacters : LeftCharacters);
            addSide = party;
        };
    }

    protected override void OnTick() {

    }

    protected override void OnFirstEnter() {

    }

    protected override void OnAnyEnter() {
        GetAll().Where(c => c.HasResource(ResourceType.CHARGE)).ToList().ForEach(c => c.Resources[ResourceType.CHARGE].IsVisible = false);
    }

    protected override void OnFirstExit() {
    }

    protected override void OnAnyExit() {

    }

    protected override void OnAddCharacter(Character c) {

    }
}
