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
using Scripts.Game.Serialized;
using Scripts.Game.Serialization;
using Scripts.Model.SaveLoad;

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
            start.Body = string.Format("Welcome to MonsterScape v{0}.", Main.VERSION);
            start.OnEnter = () => {
                List<IButtonable> buttons = new List<IButtonable>() {
                    Get(NEW_GAME),
                    Get(CREDITS),
                    new LoadPages(start),
                };
                if (Util.IS_DEBUG) {
                    buttons.Add(Get(DEBUGGING));
                }
                start.Actions = buttons;
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
                            Party party = new Party();
                            Flags flags = new Flags();
                            party.AddMember(CharacterList.Hero(name));
                            Camp camp = new Camp(party, flags);
                            bool wasCalled = false;
                            camp.Root.OnEnter += () => {
                                if (!wasCalled) {
                                    wasCalled = true;
                                    ActUtil.SetupScene(camp.Root,
                                        new TextAct("You wake up in the middle of a nicely set up campsite with a tent, fire, and backpack."),
                                        new TextAct("As you scan your surroundings for who brought you here, a <color=magenta>moving figure</color> far away catches your eye."),
                                        new TextAct("You stand up to get a better view of them, but they have already disappeared."),
                                        new TextAct("Near where you initially spotted them, you see the ruins of some town.")
                                        );
                                }
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

            Character kitsune = CharacterList.NotKitsune();
            debug.AddCharacters(Side.LEFT, kitsune);
            int level = 0;

            mainDebug.List = new IButtonable[] {
                Get(ROOT_INDEX),
                new Process("Say", "Hello", () => Presenter.Main.Instance.TextBoxes.AddTextBox(new Model.TextBoxes.TextBox("Hello"))),
                new Process("AttDisb", () => Presenter.Main.Instance.TextBoxes.AddTextBox(new Model.TextBoxes.TextBox(kitsune.Stats.LongAttributeDistribution))),
                new Process("hitsplat test", () => {
                    HitsplatView hpv = ObjectPoolManager.Instance.Get(EffectsManager.Instance.Hitsplat);
                    Util.Parent(hpv.gameObject, kitsune.Presenter.PortraitView.EffectsHolder);
                    Presenter.Main.Instance.StartCoroutine(hpv.Animation(kitsune.Presenter.PortraitView.EffectsHolder, "Test", Color.cyan, Util.GetSprite("fox-head")));
                }),
                new Battle(debug, debug, Music.BOSS, "Battle Test", new Character[] { CharacterList.Hero("Debug"), CharacterList.NotKitsune(), CharacterList.NotKitsune()  }, new Character[] { CharacterList.NotKitsune(), CharacterList.NotKitsune() }),
                new Process("LongTalk Test", () => {
                    ActUtil.SetupScene(Get(DEBUGGING), ActUtil.LongTalk(debug, kitsune, "<t>we have the best <b>guns</b><s>theaefaefef oieafoewjfoejfio oe foiawjefoawijef oj efjoiejfaoo oajeoaijfo wi best guns<a>the best gonzos the best gonzos the best gonzosthe best gonzos the best gonzos the best gonzos the best gonzos the best gonzos the best gonzos the best gonzos the best gonzos the best gonzos the best gonzos the best gonzos the best gonzos the best gonzos<a>helloworld<t>this is the captian speak"));
                }),
                new Process("Get level exp diff", () => {
                    Page.TypeText(new TextBox(string.Format("For level {0}-{1}: {2} exp", level, level + 1, Experience.GetExpDiffForLevel(level, level + 1))));
                    level++;
                }),
                new Process("ALL saves", () => SaveLoad.PrintSaves()),
                new Process("DELET all saves", () => SaveLoad.DeleteAllSaves()),
                new Process("move fox", () => { debug.Left.Clear(); debug.AddCharacters(Side.RIGHT, kitsune); }),
                new Process("test boss logo", () => ActUtil.SetupScene(debug, new BossTransitionAct(Get(CREDITS), kitsune.Look))),
                new Process("test trophy", () => GameJolt.API.Trophies.Unlock(80273)),
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
            page.Icon = Util.GetSprite("person");

            // Characters
            page.AddCharacters(Side.LEFT, new CreditsDummy(Breed.CREATOR, 5, "eternal", "spell-book", "programmer, design, and writing.\nlikes lowercase a little <i>too</i> much."));
            page.AddCharacters(Side.LEFT, new CreditsDummy(Breed.TESTER, 5, "Duperman", "shiny-apple", "Explorer of nozama."));
            page.AddCharacters(Side.LEFT, new CreditsDummy(Breed.TESTER, 99, "Rohan", "swap-bag", "Best hunter in the critically acclaimed game\n\'Ace Prunes 3\'"));
            page.AddCharacters(Side.RIGHT, new CreditsDummy(Breed.TESTER, 5, "Vishal", "round-shield", "Hacked the save file to give himself 2,147,483,647 gold in an attempt to buy the tome."));
            page.AddCharacters(Side.RIGHT, new CreditsDummy(Breed.TESTER, 5, "One of Vishal's friends", "hourglass", "Got Vitality nerfed to give 2 health, from 10."));
            page.AddCharacters(Side.RIGHT, new CreditsDummy(Breed.TESTER, 5, "cjdudeman14", "round-shield", "Open beta tester. Bug slayer. Attempted to kill that which is unkillable."));
            page.AddCharacters(Side.RIGHT, new CreditsDummy(Breed.COMMENTER, 5, "UnserZeitMrGlinko", "gladius", "\"more talking!﻿\" ~UZMG"));

            page.OnEnter += () => {
                page.AddText(
                    "<Tools>\nMade with Unity and the NUnit testing framework.",
                    "<Music>\nFrom OpenGameArt, Trevor Lentz, cynicmusic.com",
                    "<Sound Effects>\nSourced from Freesound, SoundBible, and OpenGameArt.",
                    "<Icons>\nSourced from http://Game-icons.net.",
                    "<Fonts>\nMain: BPmono by George Triantafyllakos\nTextboxes: Anonymous Pro by Mark Simonson\nHitsplat: n04b by 04\nHotkey: PKMN-Mystery-Dungeon by David Fens"
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