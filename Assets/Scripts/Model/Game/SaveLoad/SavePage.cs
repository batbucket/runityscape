using Scripts.Model.World.Pages;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.TextBoxes;
using Scripts.View.ActionGrid;
using Scripts.Presenter;
using System;

namespace Scripts.Model.World.Serialization {

    /// <summary>
    /// Page that allows users to save and overwrite data.
    /// </summary>
    public class SavePage : ReadPage {

        public SavePage(Camp camp) : base("", "Make a new save, or overwrite an existing one.", "Save", false, camp.Party, null) {
            OnTickAction += () => {
                for (int i = 0; i < SaveLoad.MAX_SAVE_FILES; i++) {
                    int index = i;
                    if (SaveLoad.IsSaveUsed(index)) {
                        Camp loadedCamp = SaveLoad.Load(i);
                        string saveName = SaveLoad.SaveFileDisplay(loadedCamp.Party.Leader.Name, loadedCamp.Party.Leader.Level);

                        ActionGrid[index] = new Process(
                            saveName,
                            "Overwrite " + saveName,
                            () => Game.Instance.CurrentPage = new ReadPage(
                                camp.Party,
                                "",
                                "Save to be overwritten: " + saveName,
                                "Overwrite this save?",
                                "",
                                new IButtonable[] {
                            new Process("Yes", "Overwrite this save.", () => {
                                Game.Instance.CurrentPage = this;
                                SaveLoad.Save(camp, index, string.Format("Overwrite into Slot {0} successful.", index));
                            }),
                            new Process("No", "Don't overwrite this save.", () => Game.Instance.CurrentPage = this) },
                                loadedCamp.Party
                            )
                        );

                    } else {
                        ActionGrid[index] = new Process(
                            string.Format("<color=grey>SLOT_{0}</color>", index),
                            "Save in an new slot.",
                            () => {
                                SaveLoad.Save(camp, index, string.Format("Save into Slot {0} successful.", index));
                            }
                            );
                    }
                }

                ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 2] = new Process(
                    "Exit without Saving",
                    "Return to the Main Menu.",
                    () => Game.Instance.CurrentPage = new ReadPage(
                            camp.Party,
                            "",
                            "Any unsaved progress will be lost.",
                            "Are you sure you want to return to the Main Menu?",
                            "",
                            new IButtonable[] {
                            new Process("Yes", "", () => Game.Instance.CurrentPage = Game.Instance.StartMenu),
                            new Process("No", "", () => Game.Instance.CurrentPage = this) }
                        )
                    );
                ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process(
                    "Return",
                    "Return to the current game.",
                    () => Game.Instance.CurrentPage = camp
                    );
            };
        }
    }
}