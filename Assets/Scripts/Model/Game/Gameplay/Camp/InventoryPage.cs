using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Items;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.TextBoxes;
using Scripts.Presenter;
using Scripts.View.ActionGrid;

namespace Scripts.Model.World.Pages {

    public class InventoryPage : ReadPage {
        private Page back;
        private Party p;

        public InventoryPage(Page back, Party p) :
            base(
            "",
            "",
            string.Format("Inventory"),
            false,
            p.Members) {
            this.back = back;
            this.p = p;

            OnEnterAction = () => {
                GenerateInventoryProcesses();
            };
        }

        private void GenerateInventoryProcesses() {
            Tooltip = string.Format("Select an item to use on a unit or toss.");
            IButtonable[] itemButtons = new IButtonable[ActionGridView.TOTAL_BUTTON_COUNT];
            int index = 0;
            foreach (Item myI in p.Inventory) {
                Item i = myI;
                itemButtons[index++] = (new Process(string.Format("{0}", i.Name), i.Description, () => GenerateUseItemProcesses(i)));
            }
            itemButtons[ActionGridView.TOTAL_BUTTON_COUNT - 1] = back;
            ActionGrid = itemButtons;
        }

        private void GenerateUseItemProcesses(Item item) {
            Tooltip = string.Format("Use {0} on who?\n{1}", item.Name, item.Description);
            IButtonable[] useButtons = new IButtonable[ActionGridView.TOTAL_BUTTON_COUNT];
            int index = 0;
            foreach (Character myC in p) {
                Character c = myC;
                useButtons[index++] = (new Process(c.DisplayName, string.Format("{0} will use {1}.\n{2}", c.DisplayName, item.Name, item.Description),
                    () => {
                        item.Cast(c, c);
                        GenerateInventoryProcesses();
                    }
                    ));
            }
            useButtons[ActionGridView.TOTAL_BUTTON_COUNT - 2]
                = new Process("Toss", string.Format("Throw {0} away.\n{1} cannot be tossed.", item.Name, Util.Color("Key items", Item.KEY_ITEM_COLOR)),
            () => {
                Game.Instance.TextBoxes.AddTextBox(
                    new TextBox(
                        string.Format("Threw away {0}.", item.Name)));
                p.Inventory.Remove(item);
                GenerateInventoryProcesses();
            },

            () => !item.IsKeyItem);
            useButtons[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Use a different item.", () => GenerateInventoryProcesses());
            ActionGrid = useButtons;
        }
    }
}