using System.Collections.Generic;

public class ExplorePage : CampOptionPage {
    public IList<PageGenerator> Explores;

    public ExplorePage(Page camp, Party party) : base(camp, party, "Where will you explore?", "Explore") {
        Explores = new PageGenerator[ActionGridView.TOTAL_BUTTON_COUNT - 1];

        OnEnterAction = () => {
            int index = 0;
            foreach (PageGenerator pg in Explores) {
                ActionGrid[index++] = pg;
            }

            ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = camp;
        };
    }
}
