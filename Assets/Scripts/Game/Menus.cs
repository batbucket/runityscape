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

namespace Scripts.Game.Pages {

    public class Menus : PageGroup {
        public const int DEBUGGING = 1;
        public const int CREDITS = 2;

        public Menus() : base(new Page("Main Menu")) {
            Register(DEBUGGING, new Page("Debug"));
            Register(CREDITS, new Page("Credits"));
            StartPage();
            DebugPage();
            CreditsPage();
        }

        private Page BasicPage(int index, int previousIndex, params IButtonable[] buttons) {
            Page page = Get(index);
            IButtonable[] list = new IButtonable[buttons.Length + 1];
            list[0] = Get(previousIndex);
            for (int i = 1; i < list.Length; i++) {
                list[i] = buttons[i];
            }
            page.Actions = list;
            return page;
        }

        private void StartPage() {
            Page start = Get(ROOT_INDEX);
            start.Actions = new IButtonable[] {
                Get(DEBUGGING),
                Get(CREDITS)
            };
        }

        private void DebugPage() {
            Page debug = Get(DEBUGGING);
            Grid submenu = new Grid("Go to submenu");
            Grid mainDebug = new Grid("Return to main menu");

            Character kitsune = new Kitsune();

            mainDebug.Array = new IButtonable[] {
                Get(ROOT_INDEX),
                new Process("Say", "Hello", () => Presenter.Main.Instance.TextBoxes.AddTextBox(new Model.TextBoxes.TextBox("Hello"))),
                new Process("AttDisb", () => Presenter.Main.Instance.TextBoxes.AddTextBox(new Model.TextBoxes.TextBox(kitsune.Stats.AttributeDistribution))),
                new Process("hitsplat test", () => {
                    HitsplatView hpv = ObjectPoolManager.Instance.Get(EffectsManager.Instance.Hitsplat);
                    Util.Parent(hpv.gameObject, kitsune.Presenter.PortraitView.EffectsHolder);
                    Presenter.Main.Instance.StartCoroutine(hpv.Animation("Test", Color.cyan, Util.GetSprite("fox-head")));
                }),
                new Battle("Battle Test", new Character[] { new Kitsune(), new Kitsune()  }, new Character[] { new Kitsune(), new Kitsune() }),
                submenu
            };

            submenu.Array = new IButtonable[] {
                new Process("Say hello", () => Presenter.Main.Instance.TextBoxes.AddTextBox(new Model.TextBoxes.TextBox("Hello"))),
                mainDebug
            };

            debug.AddCharacters(false, kitsune);
            debug.Actions = mainDebug.Array;
        }

        private void CreditsPage() {
            Page page =
                BasicPage(CREDITS, ROOT_INDEX);
            page.AddCharacters(false, new CreditsDummy(Breed.CREATOR, 5, "eternal", "spell-book", "programmer, design, and writing.\nlikes lowercase a little <i>too</i> much."));
            page.AddCharacters(false, new CreditsDummy(Breed.TESTER, 5, "Duperman", "shiny-apple", "Once explored the Amazon."));
            page.AddCharacters(false, new CreditsDummy(Breed.TESTER, 99, "Rohan", "swap-bag", "Best hunter in the critically acclaimed game\n\'Ace Prunes 3\'"));
            page.AddCharacters(true, new CreditsDummy(Breed.TESTER, 5, "Vishal", "round-shield", "Hacked the save file to give himself 2,147,483,647 gold in an attempt to buy the tome."));
            page.AddCharacters(true, new CreditsDummy(Breed.TESTER, 5, "One of Vishal's friends", "hourglass", "Name forgotten, but not gone."));
            page.AddCharacters(true, new CreditsDummy(Breed.TESTER, 5, "cjdudeman14", "tentacles-skull", "Open beta tester. Bug slayer. Attempted to kill that which is unkillable."));
            page.AddCharacters(true, new CreditsDummy(Breed.COMMENTER, 5, "UnserZeitMrGlinko", "gladius", "\"more talking!﻿\" ~UZMG"));
        }
    }

}