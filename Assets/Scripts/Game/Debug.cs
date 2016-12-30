using UnityEngine;
using System.Collections;
using System;

public class Debug : Area {
    public Page Menu;
    public Page PlayerBattle;
    public Page ComputerBattle;
    public Page BossBattle;

    private Page primary;
    public Debug(Page primary) {
        this.primary = primary;
    }

    public override void Init() {
        Character amit = new Amit();
        this.Menu = new ReadPage("What", "Hello world", left: new Character[] { new Amit() }, right: new Character[] { new Steve(), new Steve() },
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
                new Process("deadm00se", "Text not displaying right.", () => Game.Instance.TextBoxes.AddTextBox(new RightBox("placeholder", "But this world is.", Color.white))),
                new Process("Cutscene Test", action: () => {
                    Game.Instance.Cutscene(
                         false,
                         new Event(new TextBox("0 - First")),
                         new Event(new TextBox("1")),
                         new Event(new TextBox("2")),
                         new Event(new TextBox("3 - Last"))
                        );
                }),
                new Process("BossTransition", action: () => Game.Instance.Title.Play("fox-head", "Hello World")),
                new Process("Back", "Go back to the main menu.", () => { Game.Instance.CurrentPage = primary; })
    }
);

        this.PlayerBattle = new BattlePage(text: "Hello world!", mainCharacter: new Amit(), left: new Character[] { new Amit() }, right: new Character[] { new Lasher(), new Regenerator(), new Lasher() });
        this.ComputerBattle = new BattlePage(mainCharacter: new Steve(), left: new Character[] { new Amit() }, right: new Character[] { new Lasher() });
        Amit a = new Amit();
        this.BossBattle = new BattlePage(mainCharacter: a, musicLoc: "Flicker", left: new Character[] { new Regenerator(), a, new Regenerator() }, right: new Character[] { new Lasher() });
    }
}
