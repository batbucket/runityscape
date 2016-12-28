using System.Collections.Generic;

public class CampOptionPage : ReadPage {
    private Page back;

    public CampOptionPage(Page back, Party party, string tooltip, string location) : base(party, null, "", tooltip, location, null, null) {
        this.back = back;

        OnEnterAction = () => {
            ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = back;
        };
    }
}
