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

    /// <summary>
    /// Main menu.
    /// </summary>
    /// <seealso cref="Scripts.Model.Pages.PageGroup" />
    public class Menus : PageGroup {
        public const int DEBUGGING = 1;
        public const int NEW_GAME = 2;
        public const int HOTKEY_TUTORIAL = 3;

        private string name;

        public Menus() : base(new Page("Main Menu")) {
            Register(DEBUGGING, new Page("Debug"));
            Register(NEW_GAME, new Page("New Game"));
            Register(HOTKEY_TUTORIAL, new Page("Some buttons have hotkeys!"));
            StartPage();
            DebugPage();
            NewGameNameInputPage();
        }

        private string Name {
            get {
                return name;
            }
        }

        private void StartPage() {
            Page start = Get(ROOT_INDEX);
            start.Body = string.Format("Hello world.", Main.VERSION);
            start.OnEnter = () => {
                List<IButtonable> buttons = new List<IButtonable>() {
                    Get(NEW_GAME),
                    new  CreditsPages(Root),
                    new LoadPages(start),
                };
                if (Util.IS_DEBUG) {
                    buttons.Add(Get(DEBUGGING));
                }
                start.Actions = buttons;
            };
        }

        private void DebugPage() {
            Page debug = Get(DEBUGGING);
            Grid submenu = new Grid("Go to submenu");
            Grid mainDebug = new Grid("Return to main menu");

            Character kitsune = CharacterList.TestEnemy();
            debug.AddCharacters(Side.LEFT, kitsune);
            int level = 0;

            mainDebug.List = new IButtonable[] {
                Get(ROOT_INDEX),
                new Process("Say", "Hello", () => Presenter.Main.Instance.TextBoxes.AddTextBox(new Model.TextBoxes.TextBox("Hello"))),
                new Process("AttDisb", () => Presenter.Main.Instance.TextBoxes.AddTextBox(new Model.TextBoxes.TextBox(kitsune.Stats.LongAttributeDistribution))),
                new Battle(debug, debug, Music.BOSS, "Battle Test", new Character[] { CharacterList.Hero("Debug"), CharacterList.TestEnemy(), CharacterList.TestEnemy()  }, new Character[] { CharacterList.TestEnemy(), CharacterList.TestEnemy() }),
                new Process("LongTalk Test", () => {
                    ActUtil.SetupScene(ActUtil.LongTalk(debug, kitsune, "<t>we have the best <b>guns</b><s>theaefaefef oieafoewjfoejfio oe foiawjefoawijef oj efjoiejfaoo oajeoaijfo wi best guns<a>the best gonzos the best gonzos the best gonzosthe best gonzos the best gonzos the best gonzos the best gonzos the best gonzos the best gonzos the best gonzos the best gonzos the best gonzos the best gonzos the best gonzos the best gonzos the best gonzos<a>helloworld<t>this is the captian speak"));
                }),
                new Process("Get level exp diff", () => {
                    Page.TypeText(new TextBox(string.Format("For level {0}-{1}: {2} exp", level, level + 1, Experience.GetExpDiffForLevel(level, level + 1))));
                    level++;
                }),
                new Process("ALL saves", () => SaveLoad.PrintSaves()),
                new Process("DELET all saves", () => SaveLoad.DeleteAllSaves()),
                new Process("move fox", () => { debug.Left.Clear(); debug.AddCharacters(Side.RIGHT, kitsune); }),
                new Process("test boss logo", () => ActUtil.SetupScene(new BossTransitionAct(Root, kitsune.Look))),
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

        private void NewGameNameInputPage() {
            Page page = BasicPage(NEW_GAME, ROOT_INDEX,
                new Process(
                "Confirm",
                () => {
                    this.name = Get(NEW_GAME).Input;
                    HotkeyTutorialPage(name);
                    Get(HOTKEY_TUTORIAL).Invoke();
                },
                () => 2 <= Get(NEW_GAME).Input.Length && Get(NEW_GAME).Input.Length <= 10)
                );

            page.Body = "What is your name?";
            page.HasInputField = true;
        }

        private void HotkeyTutorialPage(string name) {
            Page hotkeys = Get(HOTKEY_TUTORIAL);
            hotkeys.OnEnter = (() => Util.SetCursorActive(false));
            hotkeys.Body = "Oh no! Your cursor has vanished!\n(Hint: Use your keyboard.)";
            hotkeys.Actions = new IButtonable[] {
                new Process("Advance!", () => {
                        Util.SetCursorActive(true);
                        new IntroPages(name).Invoke();
                    })
            };
        }
    }
}