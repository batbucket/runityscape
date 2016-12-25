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
        this.Menu = new ReadPage("What", "Hello world", mainCharacter: amit, left: new Character[] { amit }, right: new Character[] { new Steve(), new Steve() },
            buttonables: new Process[] {
                new Process("Normal TextBox", "Say Hello world",
                    () => Game.Instance.TextBoxes.AddTextBox(
                        new TextBox("Ethan", TextEffect.FADE_IN))),
                new Process("LeftBox", "Say Hello world",
                    () => Game.Instance.TextBoxes.AddTextBox(
                        new LeftBox("crying_mudkip", "hello", Color.white))),
                new Process("RightBox", "Say Hello world",
                    () => Game.Instance.TextBoxes.AddTextBox(
                        new RightBox("crying_mudkip", "im ethan bradberry", Color.white))),
                new Process("Test Battle", "You only <i>LOOK</i> human, don't you?", () => Page = PlayerBattle),
                new Process("Steve Massacre", "Steve. It was nice to meet you. Goodbye.", () => { Page = ComputerBattle; }),
                new Process("Shake yourself", "Literally U******E", () => { }),
                new OneShotProcess("Test OneShotProcess", "Meme", action: () => Game.Instance.TextBoxes.AddTextBox(new TextBox("hello", TextEffect.TYPE))),
                new Process("deadm00se", "Text not displaying right.", () => Game.Instance.TextBoxes.AddTextBox(new RightBox("placeholder", "But this world is.", Color.white))),
                new Process("untargetuself", "Why is this wrong", () => amit.IsTargetable = !amit.IsTargetable),
                new Process("Cutscene Test", action: () => {
                    Game.Instance.Cutscene(
                         false,
                         new Event(new TextBox("0 - First", TextEffect.TYPE)),
                         new Event(new TextBox("1", TextEffect.TYPE)),
                         new Event(new TextBox("2", TextEffect.TYPE)),
                         new Event(new TextBox("3 - Last", TextEffect.TYPE))
                        );
                }),
                new Process("BossTransition", action: () => Game.Instance.Title.Play("fox-head", "Hello World")),
                new Process("Back", "Go back to the main menu.", () => { Page = primary; })
    }
);

        this.PlayerBattle = new BattlePage(text: "Hello world!", mainCharacter: new Amit(), left: new Character[] { new Amit() }, right: new Character[] { new Lasher(), new Regenerator(), new Lasher() });
        this.ComputerBattle = new BattlePage(mainCharacter: new Steve(), left: new Character[] { new Amit() }, right: new Character[] { new Lasher() });
        Amit a = new Amit();
        this.BossBattle = new BattlePage(mainCharacter: a, musicLoc: "Flicker", left: new Character[] { new Regenerator(), a, new Regenerator() }, right: new Character[] { new Lasher() });
    }
}
