using Scripts.Model.Characters;
using Scripts.Model.World.PageGenerators;
using Scripts.Model.World.Utility;
using Scripts.Model.Pages;
using Scripts.View.ActionGrid;
using Scripts.Model.Processes;

namespace Scripts.Model.World.Pages {

    public class ExplorePage : CampOptionPage {
        private const int DISCOVERED_TEMPLE = 0;

        private EventFlags flags;

        public ExplorePage(EventFlags flags, Page camp, Party party) : base(camp, party, "Where will you explore?", "Explore") {
            this.flags = flags;
            OnEnterAction = () => {
                ActionGrid[0] = CreateExploreProcess(new Ruins(flags, camp, party));

                ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = camp;
            };
        }

        private Process CreateExploreProcess(PageGenerator pageGen) {
            return new Process(
                    pageGen.ButtonText,
                    pageGen.TooltipText,
                    () => {
                        flags.Ints[Flag.TIME]++;
                        pageGen.Invoke();
                    }
                    );
        }
    }
}