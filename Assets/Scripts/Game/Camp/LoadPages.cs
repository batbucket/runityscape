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
        private const int PASTEBIN_USAGE_COOLDOWN = 60;
        private const int IMPORT = 1;
        private static float nextTimePastebinAvailable;

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
                        buttons.Add(SetupImport(p, i));
                    }
                }

                p.Actions = buttons;
            };
        }

        private Page GetSpecificLoadPage(Page previous, int index) {
            Party party;
            Camp camp;
            string saveName;
            RestoreCamp(SaveLoad.Load(index, index.ToString()), out camp, out party, out saveName);
            Page page = new Page(saveName);
            page.Body = string.Format("What would you like to do with file {0}?", index);
            page.Actions = new IButtonable[] {
                PageUtil.GenerateBack(previous),
                GetLoadProcess(camp, index),
                PageUtil.GetConfirmationGrid(
                    previous,
                    previous,
                    GetDeleteProcess(previous, index),
                    "Delete",
                    string.Format("Delete file {0}.", index),
                    string.Format("Are you sure you want to delete file {0}?\n{1}", index, saveName)
                    ),
                GetExportProcess(previous, SaveLoad.GetSaveValue(index)),
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

        private Page SetupImport(Page previous, int index) {
            Page p = new Page(Util.ColorString("Import from Pastebin", Color.grey));
            p.Body = "Type the key associated with a Pastebin Paste containing a save string for this game."
                + "\nExample: If URL = pastebin.com/abcDEF, then type abcDEF"
                + "\nImporting from save strings created in a different version of the game may cause issues.";
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
                "Import a save from the inputted Paste key.",
                () => {
                    Main.Instance.StartCoroutine(LoadFromURL(previous, current, index));
                }
                );
        }

        private IEnumerator LoadFromURL(Page previous, Page current, int index) {
            current.AddText("Attempting to import from Pastebin...");
            string possibleSave = string.Empty;
            yield return Pastebin.GetFromKey(current.Input, s => { possibleSave = s; });
            WorldSave gameSave = null;
            try {
                gameSave = SaveLoad.Load(possibleSave, possibleSave);
            } catch (Exception e) {
                current.AddText(e.Message);
                yield break;
            }
            if (gameSave == null) {
                current.AddText("<color=red>Unable to import from the given Paste key.</color>");
            } else {
                SaveLoad.Save(gameSave, index);
                previous.Invoke();
            }
        }

        private Process GetExportProcess(Page p, string save) {
            return new Process(
                "Export to Pastebin",
                string.Format("Exports this save file to Pastebin."),
                () => {
                    p.AddText("Attempting to export...");
                    try {
                        PastebinUpload(p, save);
                    } catch (Exception e) {
                        p.AddText(e.Message);
                    }
                    nextTimePastebinAvailable = Time.time + PASTEBIN_USAGE_COOLDOWN;
                },
                () => Time.time > nextTimePastebinAvailable
                );
        }

        private void PastebinUpload(Page p, string save) {
            Pastebin.Paste(
                save,
                s => p.AddText(string.Format("Posted save to {0}.\n<color=red>The Paste will expire in 10 minutes.</color>", s))
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