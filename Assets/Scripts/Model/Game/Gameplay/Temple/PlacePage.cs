using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.Model.TextBoxes;
using Scripts.Model.World.Pages;
using Scripts.Model.World.Utility;
using Scripts.Presenter;
using Scripts.View.ActionGrid;

namespace Scripts.Model.World.Pages {

    public class PlacePage : ReadPage {

        public PlacePage(Party party, string location, string musicLoc, string text, params Character[] right)
            : base(party, musicLoc,
                text,
                "", location, null) {
            OnEnterAction += () => {
                ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new ItemManagePage(this, party);
            };
        }
    }
}