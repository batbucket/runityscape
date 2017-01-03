using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class NewGamePage : ReadPage {
    private const int LOWER_CHARACTER_LIMIT = 4;

    private bool nameMeetsRequirements {
        get {
            string[] filePaths = Directory.GetFiles(SaveLoad.SAVE_DIRECTORY);
            IList<string> takenNames = new List<string>();
            foreach (string filePath in filePaths) {
                takenNames.Add(SaveLoad.Load(filePath).Party.Leader.Name);
            }
            return InputtedString.Length >= LOWER_CHARACTER_LIMIT && !takenNames.Contains(this.InputtedString);
        }
    }

    public NewGamePage(Page back) : base(
        "Names must be at least 4 letters long and not be in the existing saves.",
        "What is your name?",
        "Name Selection",
        true) {
        PlayerCharacter hero = new Hero();
        AddCharacters(false, hero);
        OnTickAction = () => {
            hero.Name = this.InputtedString;
        };

        ActionGrid[0] = new Process("Confirm", "Choose this name.", () => Game.Instance.CurrentPage = new Camp(new Party(hero), new EventFlags()), () => nameMeetsRequirements);
        ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "", () => Game.Instance.CurrentPage = back);
    }
}
