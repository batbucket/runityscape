using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Items;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.Spells;
using Scripts.Model.TextBoxes;
using Scripts.Presenter;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Scripts.Game.Pages {
    public class InventoryPages : PageGroup {

        public InventoryPages(Page previous, Character c, Inventory inventory) : base(new Page(string.Format("Inventory ({0})", inventory.Fraction))) {
            Inventory(previous, c, inventory);
        }

        private void Inventory(Page previous, Character c, Inventory inventory) {
            Page p = Get(ROOT_INDEX);
            p.AddCharacters(Side.LEFT, c);
            p.Icon = PageUtil.INVENTORY;
            p.OnEnter = () => {
                p.AddText(string.Format("{0}/{1} spaces used.", inventory.TotalOccupiedSpace, inventory.Capacity));
                p.Actions = PageUtil.GenerateItemsGrid(
                        false,
                        p,
                        previous,
                        new SpellParams(c, p),
                        PageUtil.GetOutOfBattlePlayableHandler(p))
                        .List;
            };
        }
    }
}