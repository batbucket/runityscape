using Scripts.Model.Characters;
using Scripts.Model.World.Utility;
using Scripts.Model.Pages;
using Scripts.View.ActionGrid;
using Scripts.Model.Processes;
using Scripts.Presenter;

namespace Scripts.Model.World.Pages {

    public class PlacesPage : CampOptionPage {
        private EventFlags flags;

        public PlacesPage(EventFlags flags, Page back, Party party) : base(back, party, "Where will you go?", "Places") {
            this.flags = flags;
            OnEnterAction = () => {
                if (flags.Bools[Flag.DISCOVERED_TEMPLE]) {
                    ActionGrid[0] = CreatePlaceProcess("Temple", new TempleEntrance(back, flags, party));
                }

                ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = back;
            };
        }

        private Process CreatePlaceProcess(string name, Page page) {
            return new Process(
                    name,
                    "Go to " + name + ".",
                    () => {
                        flags.Ints[Flag.TIME]++;
                        Game.Instance.CurrentPage = page;
                    }
                    );
        }
    }
}