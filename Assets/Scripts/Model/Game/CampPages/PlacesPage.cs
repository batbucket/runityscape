using Scripts.Model.Characters;
using Scripts.Model.World.Utility;
using Scripts.Model.Pages;
using Scripts.View.ActionGrid;

namespace Scripts.Model.World.Pages {

    public class PlacesPage : CampOptionPage {

        public PlacesPage(EventFlags flags, Page back, Party party) : base(back, party, "Where will you go?", "Places") {
            OnEnterAction = () => {
                if (flags.Bools[Flag.DISCOVERED_TEMPLE]) {
                    ActionGrid[0] = new ReadPage("Temple Placeholder");
                }

                ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = back;
            };
        }
    }
}