using System.IO;

public class LoadPage : ReadPage {
    public LoadPage() : base("Select a file to load.", "", "Load", false) {
        OnEnterAction += () => {
            if (!Directory.Exists(SaveLoad.SAVE_DIRECTORY)) {
                Directory.CreateDirectory(SaveLoad.SAVE_DIRECTORY);
            }
            string[] filePaths = Directory.GetFiles(SaveLoad.SAVE_DIRECTORY, SaveLoad.LOAD_SEARCH_PATTERN);
            for (int i = 0; i < filePaths.Length; i++) {
                Camp camp = SaveLoad.Load(filePaths[i]);
                string saveName = string.Format("{0}\nLevel {1}", camp.Party.Main.Name, camp.Party.Main.Level);

                ActionGrid[i] = new Process(saveName, "Load this file.", () =>
                Game.Instance.CurrentPage = new ReadPage(
                    camp.Party,
                    "",
                    "",
                    "Load this save?",
                    "",
                    new IButtonable[] {
                        new Process("Yes", "", () => Game.Instance.CurrentPage = camp),
                        new Process("No", "", () => Game.Instance.CurrentPage = this) }
                    )
                );
            }
            ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Return to the Main Menu", () => Game.Instance.CurrentPage = Game.Instance.StartMenu);
        };
    }
}