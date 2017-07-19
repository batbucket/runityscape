using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using System.Collections.Generic;

namespace Scripts.Game.Pages {
    public class PlacePages : PageGroup {
        private readonly Page previous;
        private readonly Flags flags;
        private readonly Party party;
        private readonly IDictionary<Place, VisitablePlace> places;

        public PlacePages(Page previous, Flags flags, Party party) : base(new Page("Places")) {
            this.previous = previous;
            this.flags = flags;
            this.party = party;
            this.places = new Dictionary<Place, VisitablePlace>();
            SetupPlaces();
            SetupRoot();
        }

        private void SetupRoot() {
            Page p = Root;
            p.Body = "Where would you like to go?";
            p.AddCharacters(Side.LEFT, party);
            var buttons = new List<IButtonable>();
            buttons.Add(PageUtil.GenerateBack(previous));
            foreach (VisitablePlace vp in places.Values) {
                if (flags.UnlockedPlaces.Contains(vp.PlaceType)) {
                    buttons.Add(GetPlaceProcess(vp));
                }
            }
            p.Actions = buttons;
        }

        private Process GetPlaceProcess(VisitablePlace vp) {
            return new Process(
                vp.ButtonText,
                vp.TooltipText,
                () => {
                    flags.ShouldAdvanceTimeInCamp = true;
                    vp.Invoke();
                }
                );
        }

        private void SetupPlaces() {
            AddPlace(new Cathedral(previous, party, flags));
        }

        private void AddPlace(VisitablePlace place) {
            this.places.Add(place.PlaceType, place);
        }
    }
}