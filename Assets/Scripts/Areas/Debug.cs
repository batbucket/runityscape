using UnityEngine;
using System.Collections;
using System;

public class Debug : Area {
    public Page Menu;
    public Page PlayerBattle;
    public Page ComputerBattle;

    private Page primary;
    public Debug(Page primary) {
        this.primary = primary;
    }

    public override void Init() {
        this.Menu = new ReadPage("What", "Hello world", mainCharacter: new Amit(), left: new Character[] { new Amit() }, right: new Character[] { new Steve(), new Steve() },
            processes: new Process[] {
                new Process("Normal TextBox", "Say Hello world",
                    () => Game.AddTextBox(
                        new TextBox("Ethan", Color.white, TextEffect.FADE_IN, "Blip_0", .1f))),
                new Process("LeftBox", "Say Hello world",
                    () => Game.AddTextBox(
                        new LeftBox("crying_mudkip", "hello", Color.white))),
                new Process("RightBox", "Say Hello world",
                    () => Game.AddTextBox(
                        new RightBox("crying_mudkip", "im ethan bradberry", Color.white))),
                new Process("InputBox", "Type something",
                    () => Game.TextBoxHolder.AddInputBoxView()),
                new Process("Test Battle", "You only <i>LOOK</i> human, don't you?", () => Page = PlayerBattle),
                new Process("Steve Massacre", "Steve. It was nice to meet you. Goodbye.", () => { Page = ComputerBattle; }),
                new Process("Shake yourself", "Literally U******E", () => { }),
                new OneShotProcess("Test OneShotProcess"),
                new Process("deadm00se", "Text not displaying right.", () => Game.AddTextBox(new RightBox("placeholder", "But this world is.", Color.white))),
                new TextProcess("TextProcess", "Just TRY to spam this button", new TextBox("Hello world. Spam me.")),
                new Process("Back", "Go back to the main menu.", () => { Page = primary; })
    }
);

        this.PlayerBattle = new BattlePage(text: "Hello world!", mainCharacter: new Amit(), left: new Character[] { new Amit(), new Amit() }, right: new Character[] { new Amit(), new Steve(), new Steve() });
        this.ComputerBattle = new BattlePage(mainCharacter: new Steve(), left: new Character[] { new Steve(), new Steve(), new Steve(), new Steve(), new Steve() }, right: new Character[] { new Steve(), new Steve(), new Steve(), new Steve(), new Steve() });
    }
}
