using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

public class PagePresenter {

    public Page Page {
        set {
            SetPage(value);
        }

        get {
            return page;
        }
    }

    public string InputtedText {
        get {
            Util.Assert(inputBox != null, "Cannot get InputBox value from a Page without an InputBox!");
            return inputBox.Input;
        }
    }

    private Page page;

    private TextBoxHolderView textBoxHolder;
    private ActionGridView actionGrid;
    private PortraitHolderView left;
    private PortraitHolderView right;
    private HeaderView header;
    private TooltipView tooltip;
    private SoundView sound;

    private IList<CharacterPresenter> characterPresenters;

    private InputBoxView inputBox;

    public PagePresenter(ReadPage initial, TextBoxHolderView textBoxHolder, ActionGridView actionGrid, PortraitHolderView left, PortraitHolderView right, HeaderView header, TooltipView tooltip, SoundView sound) {
        this.textBoxHolder = textBoxHolder;
        this.actionGrid = actionGrid;
        this.left = left;
        this.right = right;
        this.header = header;
        this.tooltip = tooltip;
        this.sound = sound;
        this.characterPresenters = new List<CharacterPresenter>();
        page = new ReadPage();
        this.Page = initial; // Called last because we need the views
    }

    public void Tick() {
        Page.Tick();
        actionGrid.ClearAll();
        actionGrid.SetButtonAttributes(Page.ActionGrid);
        tooltip.PageText = Page.Tooltip;
        header.Location = Page.Location;
        SetCharacterPresenters(Page.LeftCharacters, left);
        SetCharacterPresenters(Page.RightCharacters, right);
        TickCharacterPresenters(Page.LeftCharacters.Where(c => c.IsTargetable).ToArray());
        TickCharacterPresenters(Page.RightCharacters.Where(c => c.IsTargetable).ToArray());
        Page.InputtedString = Page.HasInputField ? inputBox.Input : "";
    }

    private void SetPage(Page page) {
        this.Page.OnExit();

        if (string.IsNullOrEmpty(page.Music)) {
            sound.StopAll();
        } else if (string.Equals(page.Music, Page.Music)) {
            //Don't change music if both pages use same
        } else {
            sound.StopAll();
            sound.Loop(page.Music);
        }

        this.page = page;
        IList<Character> chars = page.GetAll();
        foreach (Character c in chars) {
            c.IsCharging = true;
        }
        Page.OnEnter();
        Page.GetAll().ForEach(c => c.BattleTimer = 0);
        Util.ReturnAllChildren(textBoxHolder.gameObject);

        Game.Instance.StopCoroutine("Timeline");
        actionGrid.IsEnabled = true;

        if (!string.IsNullOrEmpty(page.Text)) {
            AddTextBox(new TextBox(page.Text));
        }

        actionGrid.IsHotkeysEnabled = !this.Page.HasInputField;
        if (this.Page.HasInputField) {
            inputBox = textBoxHolder.AddInputBox();
            inputBox.Input = Page.InputtedString;
        } else {
            inputBox = null;
        }

        Tick();
    }

    public GameObject AddTextBox(TextBox t, Action callBack = null) {
        textBoxHolder.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        return textBoxHolder.AddTextBox(t, callBack);
    }

    private void SetCharacterPresenters(IList<Character> characters, PortraitHolderView portraitHolder) {
        IList<Character> targetableCharacters = characters.Where(c => c.IsTargetable).ToArray();
        portraitHolder.AddPortraits(targetableCharacters); //Pass in characters' Names as parameter
        foreach (Character c in targetableCharacters) {
            c.Presenter = new CharacterPresenter(c, portraitHolder.CharacterViews[c].portraitView);
            c.Presenter.PortraitView.Presenter = c.Presenter;
        }
    }

    private void TickCharacterPresenters(IList<Character> characters) {
        foreach (Character c in characters) {
            c.Presenter.Tick();
        }
    }
}
