using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

public class PagePresenter {
    public Page Page { get; set; }
    public IList<CharacterPresenter> CharacterPresenters { get; private set; }

    InputBoxView inputBox;

    /// <summary>
    /// Number of characters it takes to make a textbox as large as possible without requiring scrolling to see every character.
    /// </summary>
    private const int PERFECT_FIT_CHARACTER_COUNT = 540;
    public string InputtedText {
        get {
            Util.Assert(inputBox != null, "Cannot get InputBox value from a Page without an InputBox!");
            return inputBox.Input;
        }
    }

    public PagePresenter() {
        this.Page = new ReadPage();
        this.CharacterPresenters = new List<CharacterPresenter>();
    }

    public void SetPage(Page page) {
        this.Page.OnExit();

        if (string.IsNullOrEmpty(page.Music)) {
            Game.Instance.Sound.StopAll();
        } else if (string.Equals(page.Music, Page.Music)) {
            //Don't change music if both pages use same
        } else {
            Game.Instance.Sound.StopAll();
            Game.Instance.Sound.Loop(page.Music);
        }

        this.Page = page;
        Util.KillAllChildren(Game.Instance.TextBoxHolder.gameObject);

        if (!string.IsNullOrEmpty(page.Text)) {
            AddTextBox(new TextBox(page.Text, TextEffect.FADE_IN, "", 0));
        }

        Game.Instance.ActionGrid.HasHotkeysEnabled = !this.Page.HasInputField;
        if (this.Page.HasInputField) {
            inputBox = Game.Instance.TextBoxHolder.AddInputBoxView();
            inputBox.Input = Page.InputtedString;
        } else {
            inputBox = null;
        }

        IList<Character> chars = page.GetAll();
        foreach (Character c in chars) {
            c.Buffs.Clear();
            c.IsCharging = true;
        }

        Util.KillAllChildren(Game.Instance.LeftPortraits.gameObject);
        Util.KillAllChildren(Game.Instance.RightPortraits.gameObject);
        Game.Instance.LeftPortraits.CharacterViews.Clear();
        Game.Instance.RightPortraits.CharacterViews.Clear();

        if (page is ReadPage && page.Text.Count() > PERFECT_FIT_CHARACTER_COUNT) {
            Game.Instance.TextBoxHolder.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
        } else {
            Game.Instance.TextBoxHolder.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        }

        page.OnEnter();

        Tick();
    }

    public GameObject AddTextBox(TextBox t, Action callBack = null) {
        Game.Instance.TextBoxHolder.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        return Game.Instance.TextBoxHolder.AddTextBoxView(t, callBack);
    }

    void SetCharacterPresenters(IList<Character> characters, PortraitHolderView portraitHolder) {
        portraitHolder.AddPortraits(characters); //Pass in characters' Names as parameter
        foreach (Character c in characters) {
            c.Presenter = new CharacterPresenter(c, portraitHolder.CharacterViews[c].portraitView);
            c.Presenter.PortraitView.Presenter = c.Presenter;
        }
    }

    void TickCharacterPresenters(IList<Character> characters) {
        foreach (Character c in characters) {
            c.Presenter.Tick();
        }
    }

    public void Tick() {
        Page.Tick();
        Game.Instance.ActionGrid.ClearAll();
        Game.Instance.ActionGrid.SetButtonAttributes(Page.ActionGrid);
        Game.Instance.Tooltip.Text = Page.Tooltip;
        Game.Instance.Header.Location = Page.Location;
        SetCharacterPresenters(Page.LeftCharacters, Game.Instance.LeftPortraits);
        SetCharacterPresenters(Page.RightCharacters, Game.Instance.RightPortraits);
        TickCharacterPresenters(Page.LeftCharacters);
        TickCharacterPresenters(Page.RightCharacters);
        Page.InputtedString = Page.HasInputField ? inputBox.Input : "";
    }
}
