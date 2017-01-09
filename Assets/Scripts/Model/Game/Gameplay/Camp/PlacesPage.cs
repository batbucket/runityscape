using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.View.ActionGrid;
using Scripts.Model.Processes;
using Scripts.Presenter;
using Scripts.Model.World.Flags;

namespace Scripts.Model.World.Pages {

    public class PlacesPage : CampOptionPage {
        private EventFlags flags;

        private Camp camp;

        public PlacesPage(EventFlags flags, Camp camp, Party party) : base(camp, party, "Where will you go?", "Places") {
            this.flags = flags;
            this.camp = camp;

            OnEnterAction = () => {
                if (flags.Bools[Flag.DISCOVERED_TEMPLE]) {
                    ActionGrid[0] = CreatePlaceProcess("Temple", new TempleEntrance(camp, flags, party));
                }

                ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = camp;
            };
        }

        private Process CreatePlaceProcess(string name, Page page) {
            return new Process(
                    name,
                    "Go to " + name + ".",
                    () => {
                        camp.HasTraveled = true;
                        Game.Instance.CurrentPage = page;
                    }
                    );
        }
    }
}