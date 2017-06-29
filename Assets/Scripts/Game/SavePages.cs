using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.Game.Serialization;
using Scripts.Model.Interfaces;
using System.Collections.Generic;
using Scripts.Model.Processes;
using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Model.TextBoxes;
using Scripts.Game.Serialized;
using System;

namespace Scripts.Game.Pages {
    public class SavePages : PageGroup {

        public SavePages(Page previous, Party party, Flags flags) : base(new Page("Save Game")) {
            Setup(previous, party, flags);
        }

        private void Setup(Page previous, Party party, Flags flags) {
            Page p = Get(ROOT_INDEX);
            p.Icon = Util.GetSprite("save");
            p.OnEnter = () => {
                GameSave gs = new GameSave(party.GetSaveObject(), flags);
                p.Right.Clear();
                p.AddCharacters(Side.LEFT, party.Collection);
                p.HasInputField = false;
                List<IButtonable> buttons = new List<IButtonable>();
                buttons.Add(PageUtil.GenerateBack(previous));
                for (int i = 0; i < SaveLoad.MAX_SAVE_FILES; i++) {
                    if (SaveLoad.IsSaveUsed(i)) {
                        buttons.Add(GetOverrideSavePage(p, i, gs)); // oink
                    } else {
                        buttons.Add(
                            GetSaveProcess(
                                p,
                                string.Format("<color=grey>FILE {0}</color>", i),
                                i,
                                gs,
                                string.Format("Save to file {0}.", i),
                                string.Format("Saved to file {0}.", i)
                            )
                        );
                    }
                }
                buttons.Add(
                        PageUtil.GetConfirmationGrid(
                            p,
                            p,
                            GetExitProcess(),
                            "Exit",
                            "Return to the main menu",
                            "Are you sure you want to return to the main menu?\n<color=red>Any unsaved progress will be lost.</color>"
                            )
                    );
                p.Actions = buttons;
            };
        }

        private Page GetOverrideSavePage(Page previous, int saveIndex, GameSave save) {
            GameSave gs = SaveLoad.Load(saveIndex, saveIndex.ToString());
            Party party = new Party();
            party.InitFromSaveObject(gs.Party);
            Flags flags = gs.Flags;
            string saveName = SaveLoad.SaveFileDisplay(party.Default.Look.Name, party.Default.Stats.Level);

            Page page = PageUtil.GetConfirmationPage(
                previous,
                previous,
                GetSaveProcess(previous, "Overwrite", saveIndex, gs, string.Format("Save over file {0}.", saveIndex), string.Format("File {0} was overwritten.", saveIndex)),
                saveName,
                string.Format("Overwrite file {0}.", saveIndex),
                string.Format("Are you sure you want to overwrite file {0}?", saveIndex)
                );
            page.AddCharacters(Side.RIGHT, party.Collection);
            return page;
        }

        private Process GetSaveProcess(Page previous, string name, int saveIndex, GameSave save, string tooltip, string saveMessage) {
            return new Process(
                name,
                tooltip,
                () => {
                    SaveLoad.Save(save, saveIndex);
                    previous.Invoke();
                    previous.AddText(saveMessage);
                }
                );
        }

        private Process GetExitProcess() {
            return new Process(
                                "Exit without saving",
                                "Return to the main menu without saving.",
                                () => (new Menus()).Invoke()
                                );
        }
    }
}