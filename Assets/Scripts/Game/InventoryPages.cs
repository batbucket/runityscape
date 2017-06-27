using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Items;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using System.Collections.Generic;

namespace Scripts.Game.Pages {
    public class InventoryPages : PageGroup {
        private Inventory inventory;
        private Character character;

        public InventoryPages(Page previous, Character c, Inventory inventory) : base(new Page("Inventory")) {
            this.inventory = inventory;
            //Inventory();
        }

        private void Inventory(Page previous) {
            Page p = Get(ROOT_INDEX);
            List<IButtonable> buttons = new List<IButtonable>();
            buttons.Add(previous);
            foreach (KeyValuePair<Item, int> pair in inventory as IEnumerable<KeyValuePair<Item, int>>) {
                buttons.Add(ItemProcess(pair));
            }
            //p.Actions =
        }

        private Process ItemProcess(KeyValuePair<Item, int> pair) {
            Item item = pair.Key;
            int count = pair.Value;
            Process p = null;
            return null;
        }
    }
}