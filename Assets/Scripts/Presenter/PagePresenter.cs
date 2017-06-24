using Script.View.Tooltip;
using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.TextBoxes;
using Scripts.View.ActionGrid;
using Scripts.View.ObjectPool;
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
        private Page page;

        private TextBoxHolderView textBoxHolder;
        private ActionGridView actionGrid;
        private PortraitHolderView left;
        private PortraitHolderView right;
        private HeaderView header;
        private SoundView sound;

        private IList<CharacterPresenter> characterPresenters;

        private InputBoxView inputBox;

        public PagePresenter(Page initial, TextBoxHolderView textBoxHolder, ActionGridView actionGrid, PortraitHolderView left, PortraitHolderView right, HeaderView header, SoundView sound) {
            this.textBoxHolder = textBoxHolder;
            this.actionGrid = actionGrid;
            this.left = left;
            this.right = right;
            this.header = header;
            this.sound = sound;
            this.characterPresenters = new List<CharacterPresenter>();

            InitializeFunctions();
            this.Page = initial;
        }

        public Page Page {
            set {
                Debug.Log(value.Location);
                SetPage(value);
            }

            get {
                return page;
            }
        }

        private void InitializeFunctions() {
            Grid.ChangeGridFunc = (a => Page.Actions = a);
            Page.ChangePageFunc = (p => {
                Page = p;
            });
            Page.UpdateGridUI = p => {
                if (p == Page) {
                    actionGrid.ClearAll();
                    actionGrid.SetButtonAttributes(p.Actions);
                }
            };
            Page.TypeText = (t) => {
                return AddTextBox(t);
            };
        }

        public void Tick() {
            Page.Tick();
            header.Location = Page.Location;
            SetCharacterPresenters(Page.Left, left);
            SetCharacterPresenters(Page.Right, right);
            TickCharacterPresenters(Page.Left);
            TickCharacterPresenters(Page.Right);
        }

        private void SetPage(Page page) {

            // Music decision
            if (string.IsNullOrEmpty(page.Music)) {
                sound.StopAllSounds();
            } else if (string.Equals(page.Music, Page.Music)) {
                //Don't change music if both pages use same
            } else {
                sound.StopAllSounds();
                sound.LoopMusic(page.Music);
            }

            this.page = page;

            // Return all textboxes used
            textBoxHolder.ReturnChildren();

            // Stop any cutscenes
            Main.Instance.StopCoroutine("Timeline");

            // Renable the actiongrid in case if it was disabled
            // From Timelining
            actionGrid.IsEnabled = true;

            if (!string.IsNullOrEmpty(page.Body)) {
                AddTextBox(new TextBox(page.Body));
            }

            actionGrid.IsEnabled = !page.HasInputField;
            if (page.HasInputField) {
                textBoxHolder.AddInputBox();
            }

            actionGrid.ClearAll();
            actionGrid.SetButtonAttributes(Page.Actions);

            page.OnEnter.Invoke();

            Tick();
        }

        public PooledBehaviour AddTextBox(TextBox t, Action callBack = null) {
            textBoxHolder.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            return textBoxHolder.AddTextBox(t, callBack);
        }

        private void SetCharacterPresenters(ICollection<Character> characters, PortraitHolderView portraitHolder) {
            ICollection<Character> targetableCharacters = characters;
            portraitHolder.AddPortraits(targetableCharacters); //Pass in characters' Names as parameter
            foreach (Character c in targetableCharacters) {
                c.Presenter = new CharacterPresenter(c, portraitHolder.CharacterViews[c].portraitView);
                c.Presenter.PortraitView.Presenter = c.Presenter;
            }
        }

        private void TickCharacterPresenters(ICollection<Character> characters) {
            foreach (Character c in characters) {
                c.Presenter.Tick();
            }
        }
    }
}