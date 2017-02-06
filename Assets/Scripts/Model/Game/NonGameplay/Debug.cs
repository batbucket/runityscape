using Scripts.Model.Acts;
using Scripts.Model.Characters;
using Scripts.Model.Characters.Named;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.TextBoxes;
using Scripts.Model.World.Flags;
using Scripts.Presenter;
using System;
using UnityEngine;

namespace Scripts.Model.World.Pages {

    public class Debug : Area {
        public Page BossBattle;
        public Page ComputerBattle;
        public Page Menu;
        public Page PlayerBattle;
        private Page primary;

        public Debug(Page primary) {
            this.primary = primary;
        }

        public override void Init() {
            Character amit = new Amit();
            this.Menu = new ReadPage("What", "Hello world", "Debug", left: new Character[] { new Amit() }, right: new Character[] { new Steve(), new Steve() },
                pageActions: new PageActions() { onEnter = () => Game.Instance.Other.Camp = new Camp(new Party(new Hero()), new EventFlags()) },
                buttonables: new Process[] {
                new Process("Normal TextBox", "Say Hello world",
                    () => Game.Instance.TextBoxes.AddTextBox(
                        new TextBox("Ethan"))),
                new Process("LeftBox", "Say Hello world",
                    () => Game.Instance.TextBoxes.AddTextBox(
                        new LeftBox("crying_mudkip", "hello", Color.white))),
                new Process("RightBox", "Say Hello world",
                    () => Game.Instance.TextBoxes.AddTextBox(
                        new RightBox("crying_mudkip", "im ethan bradberry", Color.white))),
                new Process("Test Battle", "You only <i>LOOK</i> human, don't you?", () => Game.Instance.CurrentPage = PlayerBattle),
                new Process("Steve Massacre", "Steve. It was nice to meet you. Goodbye.", () => { Game.Instance.CurrentPage = ComputerBattle; }),
                new Process("Shake yourself", "Literally U******E", () => { }),
                new OneShotProcess("Test OneShotProcess", "Meme", action: () => Game.Instance.TextBoxes.AddTextBox(new TextBox("hello"))),
                new Process("print date", "", () => Game.Instance.TextBoxes.AddTextBox(new TextBox(DateTime.Now.ToString()))),
                new Process("Cutscene Test", action: () => {
                    Game.Instance.Cutscene(
                         false,
                         new Act(new TextBox("0 - First")),
                         new Act(new TextBox("1")),
                         new Act(new TextBox("2")),
                         new Act(new TextBox("3 - Last"))
                        );
                }),
                new Process("BossTransition", action: () => Game.Instance.Title.Play("fox-head", "Hello World")),
                new Process("I am boss", action: () => Game.Instance.CurrentPage = BossBattle),
                new Process("Back", "Go back to the main menu.", () => { Game.Instance.CurrentPage = primary; })
                }
            );
            Amit amity = new Amit();
            this.PlayerBattle = new BattlePage(text: "Hello world!", mainCharacter: amity, left: new Character[] { amity }, right: new Character[] { new Lasher(), new Regenerator(), new Lasher() });
            this.ComputerBattle = new BattlePage(mainCharacter: new Steve(), left: new Character[] { new Amit() }, right: new Character[] { new Lasher() });
            Amit a = new Amit();
            this.BossBattle = new BattlePage(mainCharacter: a, musicLoc: "Flicker", left: new Character[] { a }, right: new Character[] { new Shopkeeper(new EventFlags()) });
        }
    }
}