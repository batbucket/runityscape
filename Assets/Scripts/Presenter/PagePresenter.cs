using Script.View.Tooltip;
using Scripts.Game.Defined.Serialized.Characters;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
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
        private Grid overrideGrid;

        private TextBoxHolderView textBoxHolder;
        private ActionGridView actionGrid;
        private PortraitHolderView left;
        private PortraitHolderView right;
        private HeaderView header;
        private SoundView sound;

        private Func<string> GetInputFunc;

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

        public Grid Override {
            set {
                this.overrideGrid = value;
            }
            get {
                return overrideGrid;
            }
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
            Grid.ChangeGridFunc = (a => {
                Page.Actions = a;
            }
            );
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
            page.Input = GetInputFunc.Invoke();
            Page.Tick();
            header.Location = Page.Location;
            if (overrideGrid == null) {
                actionGrid.SetButtonAttributes(page.Actions);
            } else {
                actionGrid.ClearAll();
                actionGrid.SetButtonAttributes(overrideGrid.List);
            }
            SetCharacterPresenters(Page.Left, left);
            SetCharacterPresenters(Page.Right, right);
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

            if (!string.IsNullOrEmpty(page.Body)) {
                AddTextBox(new TextBox(page.Body));
            }

            actionGrid.IsEnabled = true;
            actionGrid.IsHotKeysEnabled = !page.HasInputField;
            if (page.HasInputField) {
                InputBoxView ibv = textBoxHolder.AddInputBox();
                GetInputFunc = () => ibv.Input;
            } else {
                GetInputFunc = () => string.Empty;
            }

            actionGrid.ClearAll();
            page.OnEnter.Invoke();

            Tick();
        }

        public PooledBehaviour AddTextBox(TextBox t) {
            textBoxHolder.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            return textBoxHolder.AddTextBox(t);
        }

        private void SetCharacterPresenters(IEnumerable<Character> characters, PortraitHolderView portraitHolder) {
            AddCharactersToPortraitHolder(characters, portraitHolder);
            foreach (Character c in characters) {
                c.Presenter = new CharacterPresenter(c, portraitHolder.GetPortrait(c.Id));
                c.Presenter.Tick();
            }
        }

        private void AddCharactersToPortraitHolder(IEnumerable<Character> characters, PortraitHolderView portraitHolder) {
            bool isRevealed = characters.Any(c => c.Brain is Player); // Show resources if anyone is controlled by user
            portraitHolder.AddContents(
                characters
                .Select(
                    c => new PortraitHolderView.PortraitContent() {
                        Id = c.Id,
                        Name = c.Look.DisplayName,
                        Sprite = c.Look.Sprite,
                        Tip = string.Format("Level {0} {1}\n<color=grey>{2}</color>",
                            c.Stats.Level,
                            c.Look.Breed.GetDescription(),
                            c.Look.Tooltip),
                        Buffs = c.Buffs
                            .Select(
                            b => new BuffHolderView.BuffContent() {
                                Id = b.Id,
                                Color = Color.white,
                                Description = b.Description,
                                Duration = b.DurationText,
                                Name = b.Name,
                                Sprite = b.Sprite
                            }),
                        Resources = c
                            .Stats
                            .Resources
                            .Select(
                            r => new ResourceHolderView.ResourceContent() {
                                Id = r.Type.GetHashCode(),
                                BarText = r.Mod.ToString(),
                                Numerator = r.Mod,
                                Denominator = r.Max,
                                FillColor = r.Type.Color,
                                NegativeColor = r.Type.NegativeColor,
                                Sprite = r.Type.Sprite,
                                TypeDescription = r.Type.Description,
                                Title = r.Type.Name
                            }),
                        IsRevealed = isRevealed
                    }
                ));
        }
    }
}