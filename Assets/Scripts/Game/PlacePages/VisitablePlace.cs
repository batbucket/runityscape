using Scripts.Game.Serialized;
using Scripts.Model.Pages;

namespace Scripts.Game.Pages {
    public class VisitablePlace : PageGroup {
        public readonly Place PlaceType;

        public VisitablePlace(Place type, Page root) : base(root) {
            this.PlaceType = type;
        }
    }
}