using System.Collections.Generic;

public class PlacesPage : CampOptionPage {
    public IList<Page> Places;

    public PlacesPage(Page back, Party party) : base(back, party, "Where will you go?", "Places") {
        Places = new Page[ActionGridView.TOTAL_BUTTON_COUNT - 1];

        OnEnterAction = () => {
            int index = 0;
            foreach (Page p in Places) {
                ActionGrid[index++] = p;
            }

            ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = back;
        };
    }
}
