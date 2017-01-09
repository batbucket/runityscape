using Scripts.Model.Characters;
using Scripts.Model.World.PageGenerators;
using Scripts.Model.World.Utility;
using Scripts.Model.Pages;
using Scripts.View.ActionGrid;
using Scripts.Model.Processes;
using Scripts.Model.TextBoxes;
using Scripts.Presenter;

namespace Scripts.Model.World.Pages {

    public class ExplorePage : CampOptionPage {
        private const int DISCOVERED_TEMPLE = 0;

        private EventFlags flags;
        private Camp camp;

        public ExplorePage(EventFlags flags, Camp camp, Party party) : base(camp, party, "Where will you explore?", "Explore") {
            this.flags = flags;
            this.camp = camp;
            OnEnterAction = () => {
                Game.Instance.TextBoxes.AddTextBox(new TextBox("Explorations of an area may yield differing encounters every time."));
                ActionGrid[0] = CreateExploreProcess(new Ruins(flags, camp, party));

                ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = camp;
            };
        }

        private Process CreateExploreProcess(PageGenerator pageGen) {
            return new Process(
                    pageGen.ButtonText,
                    pageGen.TooltipText,
                    () => {
                        camp.HasTraveled = true;
                        pageGen.Invoke();
                    }
                    );
        }
    }
}