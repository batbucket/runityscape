using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.View.ActionGrid;

namespace Scripts.Model.World.Pages {

    /// <summary>
    /// Represents a page accessible from Camp
    /// </summary>
    public class CampOptionPage : ReadPage {
        private Page back;

        public CampOptionPage(Page back, Party party, string tooltip, string location) : base(party, null, "", tooltip, location, null, null) {
            this.back = back;

            OnEnterAction = () => {
                ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = back;
            };
        }
    }
}