using UnityEngine;
using System.Collections;
using System;

public class StartMenu : Area {
    public Page Primary;

    private NewGame newGame;
    private Debug debug;

    public StartMenu() {
        this.newGame = new NewGame(Primary);
        this.debug = new Debug(Primary);
    }

    public override void Init() {
        Primary = new ReadPage(
            text: "Welcome to RunityScape.",
            tooltip: "Buttons can be accessed with the keyboard characters (QWERASDFZXCV)\nor by clicking.",
            processes: new Process[] {
                        new Process("New Game", "Start a new game.", () => Page = newGame.QuizIntro),
                        new Process("Load Game", "Load a saved game.", condition: () => false),
                        new Process("Debug", "Enter the debug page. ENTER AT YOUR OWN RISK.", () => Page = debug.Menu),
                        new Process(),

                        new Process(),
                        new Process(),
                        new Process(),
                        new Process(),

                        new Process(),
                        new Process(),
                        new Process(),
                        new Process("Exit", "Exit the application.", () => Application.Quit())
        });
    }
}
