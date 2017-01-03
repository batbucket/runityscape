using Scripts.Model.World.Serialization;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.TextBoxes;
using Scripts.View.ActionGrid;
using UnityEngine;
using Scripts.Presenter;

namespace Scripts.Model.World.Pages {

    public class StartMenu : ReadPage {
        private Debug debug;

        public StartMenu() : base(
                text: "Welcome to RunityScape.",
                tooltip: "Buttons can be accessed with the keyboard characters (QWERASDFZXCV) or by clicking.",
                buttonables: new Process[0]) {
            this.debug = new Debug(this);
            ActionGrid[0] = new Process("New Game", "Start a new adventure.", () => Game.Instance.CurrentPage = new NewGamePage(this));
            ActionGrid[1] = new Process("Load Game", "Load a saved game.", () => Game.Instance.CurrentPage = new LoadPage());

            if (UnityEngine.Debug.isDebugBuild) {
                ActionGrid[2] = new Process("Debug", "Enter the debug page. ENTER AT YOUR OWN RISK.", () => Game.Instance.CurrentPage = debug.Menu);
                ActionGrid[3] = new Process("Delete ALL Saves", "", () => { SaveLoad.DeleteAllSaves(); Game.Instance.TextBoxes.AddTextBox(new TextBox("Saves deleted.")); });
            }
            ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Exit", "Exit the application.", () => Application.Quit());

            OnEnterAction += () => {
                Game.Instance.Other.Camp = null;
                Game.Instance.Time.IsEnabled = false;
            };
        }
    }
}