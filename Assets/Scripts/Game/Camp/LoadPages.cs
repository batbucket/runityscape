using Scripts.Game.Serialization;
using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.SaveLoad.SaveObjects;
using Scripts.Model.TextBoxes;
using Scripts.Presenter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Pages {
    public class LoadPages : PageGroup {
        private bool isUploadingToGameJolt;

        public LoadPages(Page previous) : base(new Page("Load Game")) {
            SetupRoot(previous);
        }

        private void SetupRoot(Page previous) {
            Page p = Get(ROOT_INDEX);
            p.Icon = Util.GetSprite("load");
            p.OnEnter = () => {
                p.Left.Clear();
                p.Right.Clear();
                List<IButtonable> buttons = new List<IButtonable>();
                buttons.Add(PageUtil.GenerateBack(previous));
                for (int i = 0; i < SaveLoad.MAX_SAVE_FILES; i++) {
                    if (SaveLoad.IsSaveUsed(i)) {
                        buttons.Add(GetSpecificLoadPage(p, i));
                    } else {
                        buttons.Add(GetImportPage(p, i));
                    }
                }

                p.Actions = buttons;
            };
        }

        private Page GetSpecificLoadPage(Page previous, int index) {
            Party party;
            Camp camp;
            string saveName;
            try {
                RestoreCamp(SaveLoad.Load(index, index.ToString()), out camp, out party, out saveName);
            } catch (Exception e) {
                previous.AddText(string.Format("Save {0} is corrupted, deleting...\n{1}", index, e.Message));
                SaveLoad.DeleteSave(index);
                return GetImportPage(previous, index);
            }
            Page page = new Page(saveName);
            page.Body = string.Format("What would you like to do with file {0}?", index);
            page.Actions = new IButtonable[] {
                PageUtil.GenerateBack(previous).SetCondition(() => !isUploadingToGameJolt),
                GetLoadProcess(camp, index).SetCondition(() => !isUploadingToGameJolt),
                PageUtil.GetConfirmationGrid(
                    previous,
                    previous,
                    GetDeleteProcess(previous, index),
                    "Delete",
                    string.Format("Delete file {0}.", index),
                    string.Format("Are you sure you want to delete file {0}?\n{1}", index, saveName)
                    ).SetCondition(() => !isUploadingToGameJolt),
                GetExportPage(previous, index),
            };
            page.AddCharacters(Side.RIGHT, party.Collection);
            return page;
        }

        private void RestoreCamp(WorldSave save, out Camp camp, out Party party, out string saveName) {
            World world = new World();
            world.InitFromSaveObject(save);
            party = world.Party;
            camp = new Camp(party, world.Flags);
            saveName = SaveLoad.SaveFileDisplay(party.Default.Look.Name, party.Default.Stats.Level);
        }

        private Page GetImportPage(Page previous, int index) {
            Page p = new Page(Util.ColorString("Import", Color.grey));
            p.Body = "Type a key associated with a save for this game."
                + "\nImporting from saves created in a different version of the game may cause issues.";
            p.HasInputField = true;
            p.OnEnter = () => {
                p.Actions = new IButtonable[] {
                    PageUtil.GenerateBack(previous),
                    GetImportProcess(previous, p, index)
                };
            };
            return p;
        }

        private Process GetImportProcess(Page previous, Page current, int index) {
            return new Process(
                "Import",
                "Import a save from the inputted key.",
                () => {
                    LoadFromKey(previous, current, index, current.Input);
                }
                );
        }

        private void LoadFromKey(Page previous, Page current, int index, string key) {
            current.AddText("Attempting to import from GameJolt...");
            GameJolt.API.DataStore.Get(key, true, possibleSave => {
                if (string.IsNullOrEmpty(possibleSave)) {
                    current.AddText("Key's associated value was null.");
                } else {
                    WorldSave gameSave = null;
                    try {
                        gameSave = SaveLoad.Load(possibleSave, possibleSave);
                    } catch (Exception e) {
                        current.AddText(e.Message);
                        return;
                    }
                    SaveLoad.Save(gameSave, index);
                    previous.Invoke();
                }
            });
        }

        private Page GetExportPage(Page previous, int index) {
            Page p = new Page("Export");
            p.HasInputField = true;
            p.Body = "Type a key to associate with this save.";
            p.Actions = new IButtonable[] {
                PageUtil.GenerateBack(previous).SetCondition(() => !isUploadingToGameJolt),
                GetExportProcess(p, SaveLoad.GetSaveValue(index))
            };
            return p;
        }

        private Process GetExportProcess(Page p, string save) {
            return new Process(
                "Export",
                string.Format("Associates this save file with the provided key. The key can then be used to retrieve the save."),
                () => {
                    p.AddText("Attempting to export...");
                    try {
                        AttemptToUploadSave(p, save, p.Input);
                    } catch (Exception e) {
                        p.AddText(e.Message);
                    }
                },
                () => !isUploadingToGameJolt
                );
        }

        private void AttemptToUploadSave(Page p, string save, string key) {
            isUploadingToGameJolt = true;
            GameJolt.API.DataStore.Contains(
                key,
                true,
                isExists => {
                    if (isExists) {
                        p.AddText("The specified key already exists.");
                    } else {
                        GameJolt.API.DataStore.Set(
                            key,
                            save,
                            true,
                            isSuccessful => {
                                if (isSuccessful) {
                                    p.AddText(string.Format("Save was successfully uploaded with key: {0}", key));
                                } else {
                                    p.AddText("Unable to upload key.");
                                }
                            }
                            );
                    }
                    isUploadingToGameJolt = false;
                }
                );
        }

        private Process GetLoadProcess(Camp camp, int index) {
            return new Process(
                "Load",
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