using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterEquipsPage : ReadPage {
    private Page back;
    private Character c;

    public CharacterEquipsPage(Page back, Character c) :
        base(
        "",
        string.Format(""),
        string.Format("{0}'s Equipment", c.DisplayName),
        false,
        new Character[] { c }) {

        this.back = back;
        this.c = c;

        OnEnterAction = () => {
            Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("Inventory size: {0}/{1}", c.Items.Count, Inventory.CAPACITY)));
            DisplayEquipped();
        };
    }

    private void DisplayEquipped() {
        ActionGrid = new IButtonable[ActionGridView.TOTAL_BUTTON_COUNT];
        this.Tooltip = string.Format("Manage {0}'s equipment.", c.DisplayName);
        int index = 0;
        foreach (KeyValuePair<EquipmentType, EquippableItem> myE in c.Equipment.EquipD) {
            EquippableItem e = myE.Value;
            ActionGrid[index++] = e != null ? CreateUnequipProcess(e) : CreateEquipProcess(myE.Key);
        }
        ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = back;
    }

    private Process CreateUnequipProcess(EquippableItem item) {
        Process p = new Process(Util.Color(item.Name, c.Items.IsFull ? Color.red : Color.yellow), item.Description, () => {
            item.UnequipItemInSlot(c);
            Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("{0} unequipped <color=yellow>{1}</color>.\n{2}", c.DisplayName, item.Name, item.Description)));
            Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("Inventory size: {0}/{1}", c.Items.Count, Inventory.CAPACITY)));
            DisplayEquipped();
        }, () => !c.Items.IsFull);
        return p;
    }

    private Process CreateEquipProcess(EquipmentType type) {
        return new Process(Util.Color(string.Format("NO {0}", type.ToString()), Color.grey), string.Format("Equip an item of {0} type.", type.ToString()), () => ActionGrid = CreateEquipProcesses(type));
    }

    private Process[] CreateEquipProcesses(EquipmentType type) {
        this.Tooltip = string.Format("What equipment will go into {0}'s {1} slot?", c.DisplayName, type.ToString());
        Process[] processes = new Process[ActionGridView.TOTAL_BUTTON_COUNT];
        IList<EquippableItem> equips = c.Items.OfType<EquippableItem>().Cast<EquippableItem>().Where(i => i.EquipmentType == type).ToArray();
        int index = 0;
        foreach (EquippableItem _e in equips) {
            EquippableItem e = _e;
            processes[index++] =
                new Process(e.Name, e.Description,
                () => {
                    e.Equip(c);
                    DisplayEquipped();
                    Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("{0} equipped <color=yellow>{1}</color>.\n{2}", c.DisplayName, e.Name, e.Description)));
                    Game.Instance.TextBoxes.AddTextBox(new TextBox(string.Format("Inventory size: {0}/{1}", c.Items.Count, Inventory.CAPACITY)));
                }
            );
        }
        processes[ActionGridView.TOTAL_BUTTON_COUNT - 1] = new Process("Back", string.Format("View the rest of {0}'s equipment.", c.DisplayName), () => DisplayEquipped());
        return processes;
    }
}
