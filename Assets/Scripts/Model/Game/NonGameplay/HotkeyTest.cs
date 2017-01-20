using UnityEngine;
using System.Collections;
using Scripts.Model.Pages;
using Scripts.View.ActionGrid;
using Scripts.Model.Processes;
using Scripts.Presenter;
using Scripts.Model.TextBoxes;

namespace Scripts.Model.World.Pages {

    public class HotkeyTest : ReadPage {
        public HotkeyTest(Page back) : base(location: "Hotkey Test", tooltip: "Press TAB to leave.") {
            for (int i = 0; i < ActionGridView.TOTAL_BUTTON_COUNT; i++) {
                string hotkey = ActionGridView.HOTKEYS[i].ToString();
                ActionGrid[i] = new Process(
                        hotkey,
                        "",
                        () => Game.Instance.TextBoxes.AddTextBox(new TextBox(hotkey))
                    );
            }

            OnTickAction += () => {
                if (Input.GetKeyDown(KeyCode.Tab)) {
                    Game.Instance.CurrentPage = back;
                }
            };
        }
    }
}