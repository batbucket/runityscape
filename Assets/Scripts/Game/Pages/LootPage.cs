using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootPage : ReadPage {
    public bool HasLoot {
        get {
            return loot.Count > 0;
        }
    }
    private IList<Item> loot;
    private Inventory inventory;
    private Page back;

    public LootPage(BattlePage back, Party looters, IList<Character> defeated) : base(looters, "", "", "", "Looting", null, null) {
        this.back = back;
        this.inventory = looters.Inventory;
        this.loot = ExtractLoot(defeated);
        OnTickAction += () => {
            this.ClearActionGrid();
            CreateTooltip();
            DisplayLoot();
            ActionGrid[ActionGridView.TOTAL_BUTTON_COUNT - 1] = back;
        };
    }

    private void CreateTooltip() {
        Tooltip = inventory.SizeText;
        if (inventory.IsFull) {
            Tooltip += "\n<color=red>Your inventory is full.</color>";
        }
    }

    private IList<Item> ExtractLoot(IList<Character> defeated) {
        IList<Item> loot = new List<Item>();

        // Get inventories and equips
        foreach (Character _c in defeated) {
            Character c = _c;
            foreach (Item i in c.Inventory) {
                loot.Add(i);
            }
            foreach (EquippableItem _e in c.Equipment) {
                EquippableItem e = _e;
                if (e != null) {
                    loot.Add(e);
                }
            }
        }

        return loot;
    }

    private void DisplayLoot() {
        for (int i = 0; i < ActionGridView.TOTAL_BUTTON_COUNT - 1 && i < loot.Count; i++) {
            ActionGrid[i] = LootProcess(loot[i]);
        }
    }

    private Process LootProcess(Item i) {
        return new Process(i.Name, string.Format("Loot {0}.\n{1}", i.Name, i.Description), () => {
            inventory.Add(i);
            loot.Remove(i);
        }
            ,
            () => !inventory.IsFull
        );
    }
}
