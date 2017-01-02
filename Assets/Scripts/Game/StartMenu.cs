using UnityEngine;
using System.Collections;
using System;

public class StartMenu : ReadPage {
    private Debug debug;

    public StartMenu() : base(
            text: "Welcome to RunityScape.",
            tooltip: "Buttons can be accessed with the keyboard characters (QWERASDFZXCV) or by clicking.",
            buttonables: new Process[0]) {

        this.debug = new Debug(this);
        ActionGrid[0] = new Process("New Game", "Start a new adventure.", () => Game.Instance.CurrentPage = new NewGamePage(this));
        ActionGrid[1] = new Process("Load Game", "Load a saved game.", () => Game.Instance.CurrentPage = new LoadPage());

        if (Application.isEditor) {
            ActionGrid[2] = new Process("Debug", "Enter the debug page. ENTER AT YOUR OWN RISK.", () => Game.Instance.CurrentPage = debug.Menu);
        }
        ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Exit", "Exit the application.", () => Application.Quit());
    }
}
