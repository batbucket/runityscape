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
            int revealContributionFromFlags = Convert.ToInt32(characters.Any(c => c.Brain is Player)); // Being in same party as player gives +1 to revealing
            portraitHolder.AddContents(
                characters
                .Select(
                    c => new PortraitHolderView.PortraitContent(
                        id: c.Id,
                        name: c.Look.DisplayName,
                        sprite: c.Look.Sprite,
                        tip: string.Format("Level {0} {1}\n<color=grey>{2}</color>",
                            c.Stats.Level,
                            c.Look.Breed.GetDescription(),
                            c.Look.Tooltip),
                        buffs: c.Buffs
                            .Select(
                            b => new BuffHolderView.BuffContent(
                                id: b.Id,
                                color: Color.white,
                                description: b.Description,
                                duration: b.DurationText,
                                name: b.Name,
                                sprite: b.Sprite
                            )),
                        resources: c
                            .Stats
                            .Resources
                            .Select(
                            r => new ResourceHolderView.ResourceContent(
                                id: r.Type.GetHashCode(),
                                barText: r.Mod.ToString(),
                                numerator: r.Mod,
                                denominator: r.Max,
                                fillColor: r.Type.Color,
                                negativeColor: r.Type.NegativeColor,
                                sprite: r.Type.Sprite,
                                typeDescription: r.Type.Description,
                                title: r.Type.Name
                            )),
                        isRevealed: IsRevealedCalculation(c.Stats, revealContributionFromFlags)
                    )
                )
            );
        }

        private bool IsRevealedCalculation(Stats stats, int contributionFromFlags) {
            return stats.ResourceVisibility + contributionFromFlags > 0;
        }
    }
}