using Script.View.Tooltip;
using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.Model.TextBoxes;
using Scripts.View.ActionGrid;
using Scripts.View.Other;
using Scripts.View.Portraits;
using Scripts.View.Sounds;
using Scripts.View.TextBoxes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Presenter {

    /// <summary>
    /// Connection between things on the screen
    /// and the page model.
    /// </summary>
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
        private SoundView sound;

        private IList<CharacterPresenter> characterPresenters;

        private InputBoxView inputBox;

        public PagePresenter(ReadPage initial, TextBoxHolderView textBoxHolder, ActionGridView actionGrid, PortraitHolderView left, PortraitHolderView right, HeaderView header, SoundView sound) {
            this.textBoxHolder = textBoxHolder;
            this.actionGrid = actionGrid;
            this.left = left;
            this.right = right;
            this.header = header;
            this.sound = sound;
            this.characterPresenters = new List<CharacterPresenter>();
            page = new ReadPage();
            this.Page = initial; // Called last because we need the views
        }

        public void Tick() {
            Page.Tick();
            actionGrid.ClearAll();
            actionGrid.SetButtonAttributes(Page.ActionGrid);
            header.Location = Page.Location;
            SetCharacterPresenters(Page.LeftCharacters, left);
            SetCharacterPresenters(Page.RightCharacters, right);
            TickCharacterPresenters(Page.LeftCharacters.Where(c => c.IsTargetable).ToArray());
            TickCharacterPresenters(Page.RightCharacters.Where(c => c.IsTargetable).ToArray());
            Page.InputtedString = Page.HasInputField ? inputBox.Input : "";
        }

        private void SetPage(Page page) {
            this.Page.Exit();

            // Music decision
            if (string.IsNullOrEmpty(page.Music)) {
                sound.StopAllSounds();
            } else if (string.Equals(page.Music, Page.Music)) {
                //Don't change music if both pages use same
            } else {
                sound.StopAllSounds();
                sound.LoopMusic(page.Music);
            }

            // Restore charging if defeated
            this.page = page;
            IList<Character> chars = page.GetAll();
            foreach (Character c in chars) {
                c.IsCharging = true;
            }

            // Reset battle timer
            Page.GetAll().ForEach(c => c.BattleTimer = 0);

            // Return all textboxes used
            textBoxHolder.ReturnChildren();

            // Stop any cutscenes
            Game.Instance.StopCoroutine("Timeline");

            // Renable the actiongrid in case if it was disabled
            // From Timelining
            actionGrid.IsEnabled = true;

            if (!string.IsNullOrEmpty(page.Text)) {
                AddTextBox(new TextBox(page.Text));
            }

            // Only allow hotkeyable buttons on pages without an input field
            actionGrid.IsHotkeysEnabled = !this.Page.HasInputField;
            if (this.Page.HasInputField) {
                inputBox = textBoxHolder.AddInputBox();
                inputBox.Input = Page.InputtedString;
            } else {
                inputBox = null;
            }

            page.Enter();
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
}