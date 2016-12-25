using UnityEngine;
using System.Collections;
using System;

public class StartMenu : Area {
    public ReadPage MainMenu;

    private NewGame newGame;
    private Debug debug;

    public StartMenu() {
        this.newGame = new NewGame(MainMenu);
        this.debug = new Debug(MainMenu);
    }

    public override void Init() {
        MainMenu = new ReadPage(
            text: "Welcome to RunityScape.",
            tooltip: "Buttons can be accessed with the keyboard characters (QWERASDFZXCV) or by clicking.",
            buttonables: new Process[] {
                        new Process("New Game", "Start a new adventure.", () => Page = newGame.QuizIntro),
                        new Process("Load Game", "Load a saved game.", playCondition: () => false),
                        Application.isEditor ? new Process("Debug", "Enter the debug page. ENTER AT YOUR OWN RISK.", () => Page = debug.Menu) : new Process(),
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
