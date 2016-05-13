using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class PagePresenter {
    public Page Page { get; set; }
    public IList<CharacterPresenter> CharacterPresenters { get; private set; }
    IDictionary<Page, IList<TextBox>> textBoxHistory;

    InputBoxView inputBox;
    public string InputtedText {
        get {
            Util.Assert(inputBox != null, "Cannot get InputBox value from a Page without an InputBox!");
            return inputBox.Input;
        }
    }

    public PagePresenter() {
        this.Page = new ReadPage();
        this.CharacterPresenters = new List<CharacterPresenter>();
        this.textBoxHistory = new Dictionary<Page, IList<TextBox>>();
    }

    public void SetPage(Page page) {

        this.Page = page;
        Util.KillAllChildren(Game.Instance.TextBoxHolder.gameObject);

        if (!textBoxHistory.ContainsKey(page)) {
            textBoxHistory.Add(page, new List<TextBox>());
            if (!string.IsNullOrEmpty(Page.Text)) {
                AddTextBox(new TextBox(Page.Text, Color.white, TextEffect.FADE_IN));
            }
        } else {
            IList<TextBox> textBoxes = textBoxHistory[page];
            foreach (TextBox t in textBoxes) {
                Game.Instance.TextBoxHolder.AddTextBoxView(t);
            }
        }

        Game.Instance.ActionGrid.HasHotkeysEnabled = !this.Page.HasInputField;
        if (this.Page.HasInputField) {
            inputBox = Game.Instance.TextBoxHolder.AddInputBoxView();
            inputBox.Input = Page.InputtedString;
        } else {
            inputBox = null;
        }

        this.Page.OnExit();
        page.OnEnter();
    }

    public void AddTextBox(TextBox t, Action callBack = null) {
        textBoxHistory[Page].Add(t);
        Game.Instance.TextBoxHolder.AddTextBoxView(t, callBack);
    }

    void SetCharacterPresenters(IList<Character> characters, PortraitHolderView portraitHolder) {
        portraitHolder.AddPortraits(characters.Select(c => c.Name).ToArray()); //Pass in characters' Names as parameter
        foreach (Character c in characters) {
            c.Presenter = new CharacterPresenter(c, portraitHolder.CharacterViews[c.Name].portraitView);
        }
    }

    void TickCharacterPresenters(IList<Character> characters) {
        foreach (Character c in characters) {
            c.Presenter.Tick();
        }
    }

    public void Tick() {
        Game.Instance.ActionGrid.ClearAll();
        Game.Instance.ActionGrid.SetButtonAttributes(Page.ActionGrid);
        Game.Instance.Tooltip.Text = Page.Tooltip;
        Game.Instance.Header.Location = Page.Location;
        SetCharacterPresenters(Page.LeftCharacters, Game.Instance.LeftPortraits);
        SetCharacterPresenters(Page.RightCharacters, Game.Instance.RightPortraits);
        TickCharacterPresenters(Page.LeftCharacters);
        TickCharacterPresenters(Page.RightCharacters);
        Page.InputtedString = Page.HasInputField ? inputBox.Input : "";
        Page.Tick();
    }
}
