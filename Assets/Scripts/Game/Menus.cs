using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.Characters;
using Scripts.Presenter;
using UnityEngine;
using Scripts.View.Effects;
using Scripts.View.ObjectPool;
using Scripts.Game.Defined.Characters;
using System;
using Scripts.Game.Defined.Spells;
using Scripts.Game.Defined.Serialized.Spells;
using System.Collections.Generic;
using Scripts.Game.Undefined.Characters;
using Scripts.Model.TextBoxes;
using Scripts.Model.Acts;
using Scripts.Model.Stats;
using Scripts.Game.Defined.Serialized.Statistics;

namespace Scripts.Game.Pages {

    public class Menus : PageGroup {
        public const int DEBUGGING = 1;
        public const int CREDITS = 2;
        public const int NEW_GAME = 3;
        public const int INTRO_SCENE = 4;

        private string name;

        public Menus() : base(new Page("Main Menu")) {
            Register(DEBUGGING, new Page("Debug"));
            Register(CREDITS, new Page("Credits"));
            Register(NEW_GAME, new Page("New Game"));
            Register(INTRO_SCENE, new Page("Unknown"));
            StartPage();
            DebugPage();
            CreditsPage();
            SetupIntro();
            NewGameNameInputPage();
        }

        private string Name {
            get {
                return name;
            }
        }

        private void StartPage() {
            Page start = Get(ROOT_INDEX);
            start.Actions = new IButtonable[] {
                Get(NEW_GAME),
                Get(DEBUGGING),
                Get(CREDITS)
            };
        }

        private void SetupIntro() {
            Page page = Get(INTRO_SCENE);
            page.OnEnter = () => {
                ActUtil.SetupScene(page,
                        IntroVoice("{0}...", Name),
                        IntroVoice("Fallen hero of humanity..."),
                        IntroVoice("Your time on this world is not yet over."),
                        IntroVoice("Arise once more and bring us to egress..."),
                        new TextAct("You feel like you are being dragged off somewhere..."),
                        new ActionAct(() => {
                            Camp camp = new Camp(name);
                            camp.Root.OnEnter += () => {
                                ActUtil.SetupScene(camp.Root,
                                    new TextAct("You wake up in the middle of a nicely set up campsite with a tent, fire, and backpack."),
                                    new TextAct("As you scan your surroundings for who brought you here, a <color=magenta>moving figure</color> far away catches your eye."),
                                    new TextAct("You stand up to get a better view of them, but they have already disappeared."),
                                    new TextAct("Near where you initially spotted them, you see the ruins of some town.")
                                    );
                                camp.Root.OnEnter = () => { };
                            };
                            camp.Root.Invoke();
                        })
                    );
            };
        }

        private TextAct IntroVoice(string message, object arg = null) {
            return new TextAct(new AvatarBox(Side.RIGHT, Util.GetSprite("holy-symbol"), Color.yellow, string.Format(message, arg)));
        }

        private void DebugPage() {
            Page debug = Get(DEBUGGING);
            Grid submenu = new Grid("Go to submenu");
            Grid mainDebug = new Grid("Return to main menu");

            Character kitsune = new Kitsune();
            debug.AddCharacters(Side.LEFT, kitsune);
            int level = 0;

            mainDebug.List = new IButtonable[] {
                Get(ROOT_INDEX),
                new Process("Say", "Hello", () => Presenter.Main.Instance.TextBoxes.AddTextBox(new Model.TextBoxes.TextBox("Hello"))),
                new Process("AttDisb", () => Presenter.Main.Instance.TextBoxes.AddTextBox(new Model.TextBoxes.TextBox(kitsune.Stats.LongAttributeDistribution))),
                new Process("hitsplat test", () => {
                    HitsplatView hpv = ObjectPoolManager.Instance.Get(EffectsManager.Instance.Hitsplat);
                    Util.Parent(hpv.gameObject, kitsune.Presenter.PortraitView.EffectsHolder);
                    Presenter.Main.Instance.StartCoroutine(hpv.Animation("Test", Color.cyan, Util.GetSprite("fox-head")));
                }),
                new Battle("Battle Test", new Character[] { new Kitsune(), new Kitsune()  }, new Character[] { new Kitsune(), new Kitsune() }),
                new Process("LongTalk Test", () => {
                    ActUtil.SetupScene(Get(DEBUGGING), ActUtil.LongTalk(debug, kitsune, "<t>we have the best <b>guns</b><s>the best guns<a>the best gonzos the best gonzos the best gonzosthe best gonzos the best gonzos the best gonzos<a>helloworld<t>this is the captian speak"));
                }),
                new Process("Get level exp diff", () => {
                    Page.TypeText(new TextBox(string.Format("For level {0}-{1}: {2} exp", level, level + 1, Experience.GetExpDiffForLevel(level, level + 1))));
                    level++;
                }),
                submenu
            };

            submenu.List = new IButtonable[] {
                new Process("Say hello", () => Presenter.Main.Instance.TextBoxes.AddTextBox(new Model.TextBoxes.TextBox("Hello"))),
                mainDebug
            };

            debug.AddCharacters(Side.LEFT, kitsune);
            debug.Actions = mainDebug.List;
        }

        private void CreditsPage() {
            Page page =
                BasicPage(CREDITS, ROOT_INDEX);

            // Characters
            page.AddCharacters(Side.LEFT, new CreditsDummy(Breed.CREATOR, 5, "eternal", "spell-book", "programmer, design, and writing.\nlikes lowercase a little <i>too</i> much."));
            page.AddCharacters(Side.LEFT, new CreditsDummy(Breed.TESTER, 5, "Duperman", "shiny-apple", "Once explored the Amazon."));
            page.AddCharacters(Side.LEFT, new CreditsDummy(Breed.TESTER, 99, "Rohan", "swap-bag", "Best hunter in the critically acclaimed game\n\'Ace Prunes 3\'"));
            page.AddCharacters(Side.RIGHT, new CreditsDummy(Breed.TESTER, 5, "Vishal", "round-shield", "Hacked the save file to give himself 2,147,483,647 gold in an attempt to buy the tome."));
            page.AddCharacters(Side.RIGHT, new CreditsDummy(Breed.TESTER, 5, "One of Vishal's friends", "hourglass", "Name forgotten, but not gone."));
            page.AddCharacters(Side.RIGHT, new CreditsDummy(Breed.TESTER, 5, "cjdudeman14", "tentacles-skull", "Open beta tester. Bug slayer. Attempted to kill that which is unkillable."));
            page.AddCharacters(Side.RIGHT, new CreditsDummy(Breed.COMMENTER, 5, "UnserZeitMrGlinko", "gladius", "\"more talking!﻿\" ~UZMG"));

            page.OnEnter += () => {
                page.AddText(
                    "Tools: Made with Unity and the NUnit testing framework.",
                    "Music: Sourced from OpenGameArt.",
                    "Sound Effects: Sourced from Freesound, SoundBible, and OpenGameArt.",
                    "Icons: Sourced from http://Game-icons.net.",
                    "Fonts: Determination Mono (Main), Hachicro (Hitsplats), Mars Needs C. (Bar text)"
                    );
            };
        }

        private void NewGameNameInputPage() {
            Page page = BasicPage(NEW_GAME, ROOT_INDEX,
                new Process(
                "Confirm",
                () => {
                    this.name = Get(NEW_GAME).Input;
                    SetupIntro();
                    Get(INTRO_SCENE).Invoke();
                },
                () => 2 <= Get(NEW_GAME).Input.Length && Get(NEW_GAME).Input.Length <= 10)
                );

            page.Body = "What was the hero's name?";
            page.HasInputField = true;
        }
    }

}