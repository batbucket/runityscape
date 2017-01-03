using Scripts.Model.Characters;
using Scripts.Model.World.PageGenerators;
using Scripts.Model.World.Utility;
using Scripts.Model.Pages;
using Scripts.View.ActionGrid;

namespace Scripts.Model.World.Pages {

    public class ExplorePage : CampOptionPage {
        private const int DISCOVERED_TEMPLE = 0;

        public ExplorePage(EventFlags flags, Page camp, Party party) : base(camp, party, "Where will you explore?", "Explore") {
            OnEnterAction = () => {
                ActionGrid[0] = new Ruins(flags, camp, party);

                ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = camp;
            };
        }
    }
}