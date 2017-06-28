using Scripts.Game.Serialization;
using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Model.TextBoxes;
using System.Collections.Generic;

namespace Scripts.Game.Pages {
    public class LoadPages : PageGroup {
        public LoadPages(Page previous) : base(new Page("Load Game")) {
            Setup(previous);
        }

        private void Setup(Page previous) {
            Page p = Get(ROOT_INDEX);
            p.Icon = Util.GetSprite("load");
            p.OnEnter = () => {
                p.Right.Clear();
                List<IButtonable> buttons = new List<IButtonable>();
                buttons.Add(PageUtil.GenerateBack(previous));
                for (int i = 0; i < SaveLoad.MAX_SAVE_FILES; i++) {
                    if (SaveLoad.IsSaveUsed(i)) {
                        GameSave save = SaveLoad.Load(i, SaveLoad.IS_ENCRYPT_VALUES);
                        Party party = new Party();
                        party.InitFromSaveObject(save.Party);
                        Flags flags = save.Flags;
                        Camp camp = new Camp(party, flags);
                        string saveName = SaveLoad.SaveFileDisplay(party.Default.Look.Name, party.Default.Stats.Level);

                        Grid grid = PageUtil.GetConfirmationGrid(
                                p,
                                p,
                                GetLoadProcess(camp, i),
                                saveName,
                                string.Format("Load from file {0}.", i),
                                string.Format("Are you sure you want to load file {0}?\n{1}", i, saveName)
                                );

                        grid.List.Add(
                                PageUtil.GetConfirmationGrid(
                                p,
                                p,
                                GetDeleteProcess(p, i),
                                "<color=red>Delete</color>",
                                string.Format("Delete file {0}.", i),
                                string.Format("Are you sure you want to delete file {0}?\n{1}", i, saveName)
                                )
                            );
                        grid.OnEnter = () => {
                            p.AddCharacters(Side.RIGHT, party.Collection);
                        };

                        buttons.Add(grid);
                    }
                }

                p.Actions = buttons;
            };
        }

        private Process GetLoadProcess(Camp camp, int index) {
            return new Process(
                "Yes",
                "Load this save file.",
                () => {
                    camp.Invoke();
                    camp.Root.AddText(string.Format("Loaded from save file {0}.", index));
                }
                );
        }

        private Process GetDeleteProcess(Page previous, int index) {
            return new Process(
                "<color=red>Delete</color>",
                "Delete this save file.",
                () => {
                    SaveLoad.DeleteSave(index);
                    previous.Invoke();
                    previous.AddText(string.Format("File {0} was deleted.", index));
                }
                );
        }
    }
}