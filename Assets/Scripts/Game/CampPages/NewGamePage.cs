using UnityEngine;
using System.Collections;

public class NewGamePage : ReadPage {
    private const int LOWER_CHARACTER_LIMIT = 4;

    public NewGamePage(Page back) : base(
        "Names must be at least 4 letters long.",
        "What is your name?",
        "Name Selection",
        true) {
        PlayerCharacter hero = new Hero();
        AddCharacters(false, hero);
        OnTickAction = () => {
            hero.Name = this.InputtedString;
        };

        ActionGrid[0] = new Process("Confirm", "Choose this name.", () => Game.Instance.CurrentPage = new Camp(hero), () => InputtedString.Length >= LOWER_CHARACTER_LIMIT);
        ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "", () => Game.Instance.CurrentPage = back);
    }
}
