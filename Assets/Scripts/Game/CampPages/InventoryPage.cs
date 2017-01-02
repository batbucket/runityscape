using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryPage : ReadPage {
    private Party p;
    private Page back;

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
        useButtons[ActionGridView.TOTAL_BUTTON_COUNT - 2] = new Process("Toss", string.Format("Throw {0} away.", item.Name),
        () => {
            Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("Threw away {0}.", item.Name)));
            p.Inventory.Remove(item);
            GenerateInventoryProcesses();

        });
        useButtons[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", "Use a different item.", () => GenerateInventoryProcesses());
        ActionGrid = useButtons;
    }
}
