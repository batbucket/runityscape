public class ItemManagePage : CampOptionPage {
    public ItemManagePage(Page back, Party party) : base(back, party, "Manage a unit's equipment, or use items in inventory.", "Item Manager") {
        OnEnterAction = () => {
            int index = 0;
            foreach (Character myC in party) {
                Character c = myC;
                ActionGrid[index++] = new CharacterEquipsPage(this, c);
            }

            ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 2] = new InventoryPage(this, party);
            ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = back;
        };
    }
}
