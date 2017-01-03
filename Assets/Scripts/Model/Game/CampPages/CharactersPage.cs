using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.View.ActionGrid;

namespace Scripts.Model.World.Pages {

    public class CharactersPage : CampOptionPage {

        public CharactersPage(Page back, Party party) : base(back, party, "View a unit's stats or perform level-ups.", "Characters") {
            OnEnterAction = () => {
                int index = 0;
                foreach (Character myc in party) {
                    Character c = myc;
                    ActionGrid[index++] = new CharacterStatsPage(this, c);
                }
                ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = back;
            };
        }
    }
}