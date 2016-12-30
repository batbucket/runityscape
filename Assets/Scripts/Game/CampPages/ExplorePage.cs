using System.Collections.Generic;

public class ExplorePage : CampOptionPage {
    private const int DISCOVERED_TEMPLE = 0;

    public ExplorePage(Flags flags, Page camp, Party party) : base(camp, party, "Where will you explore?", "Explore") {

        OnEnterAction = () => {
            ActionGrid[0] = new Ruins(flags, camp, party);

            ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = camp;
        };
    }
}
