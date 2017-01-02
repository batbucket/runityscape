using System.IO;

public class SavePage : ReadPage {

    public SavePage(Camp camp) : base("", "Make a new save, or overwrite an existing one.", "Save", false, camp.Party, null) {
        OnTickAction += () => {
            string[] filePaths = Directory.GetFiles(SaveLoad.SAVE_DIRECTORY, SaveLoad.LOAD_SEARCH_PATTERN);
            for (int i = 0; i < filePaths.Length; i++) {
                Camp loadedCamp = SaveLoad.Load(filePaths[i]);
                string saveName = string.Format("{0}\nLevel {1}", camp.Party.Main.Name, camp.Party.Main.Level);

                ActionGrid[i] = new Process(
                    saveName,
                    "",
                    () => Game.Instance.CurrentPage = new ReadPage(
                        camp.Party,
                        "",
                        "Save to be overwritten: " + saveName,
                        "Overwrite this save?",
                        "",
                        new IButtonable[] {
                            new Process("Yes", "", () => SaveLoad.Save(camp, filePaths[i], true)),
                            new Process("No", "", () => Game.Instance.CurrentPage = this) },
                        loadedCamp.Party
                    )
                );
            }

            for (int i = filePaths.Length; i < filePaths.Length - SaveLoad.MAX_SAVE_FILES; i++) {
                ActionGrid[i] = new Process(
                    "EMPTY",
                    "Create a new save.",
                    () => SaveLoad.Save(camp, string.Format("{0}\\{1}", SaveLoad.SAVE_DIRECTORY, camp.Party.Main.Name), false)
                    );
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
