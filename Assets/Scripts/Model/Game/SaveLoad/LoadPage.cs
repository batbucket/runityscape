using Scripts.Model.World.Pages;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.View.ActionGrid;
using Scripts.Presenter;
using System;
using Scripts.Model.TextBoxes;

namespace Scripts.Model.World.Serialization {

    /// <summary>
    /// Page that allows users to load their saved games.
    /// </summary>
    public class LoadPage : ReadPage {

        public LoadPage() : base("Select a file to load.", "", "Load", false) {
            OnTickAction += () => {
                for (int i = 0; i < SaveLoad.MAX_SAVE_FILES; i++) {
                    int index = i;
                    if (SaveLoad.IsSaveUsed(i)) {
                        Camp camp = SaveLoad.Load(i);
                        string saveName = SaveLoad.SaveFileDisplay(camp.Party.Leader.Name, camp.Party.Leader.Level);

                        ActionGrid[i] = new Process(saveName, "Load this file.", () =>
                        Game.Instance.CurrentPage = new ReadPage(
                            camp.Party,
                            "",
                            "",
                            "Load this save?",
                            "",
                            new IButtonable[] {
                        new Process("Yes", "", () => Game.Instance.CurrentPage = camp),
                        new Process("No", "", () => Game.Instance.CurrentPage = this),
                        new Process(),
                        new Process(),

                        new Process(),
                        new Process(),
                        new Process(),
                        new Process(),

                        new Process("Delete", "Delete this save.", () => {
                                Game.Instance.CurrentPage = this;
                                SaveLoad.DeleteSave(index,
                                    string.Format("{0} (Lvl {1}) was deleted.",
                                    camp.Party.Leader.Name,
                                    camp.Party.Leader.Level));
                            }
                            )
                            }
                        ));
                    }
                }
                ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Return to the Main Menu", () => Game.Instance.CurrentPage = Game.Instance.StartMenu);
            };
        }
    }
}