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

        public InventoryPages(Page previous, Party party, Inventory inventory) : base(new Page(string.Format("Inventory ({0})", inventory.Fraction))) {
            Inventory(previous, party, inventory);
        }

        private void Inventory(Page previous, Party party, Inventory inventory) {
            Page p = Get(ROOT_INDEX);
            p.AddCharacters(Side.LEFT, party);
            p.Icon = PageUtil.INVENTORY;
            p.OnEnter = () => {
                p.AddText(string.Format("{0}/{1} spaces used.", inventory.TotalOccupiedSpace, inventory.Capacity));
                p.Actions = PageUtil.GenerateItemsGrid(
                        false,
                        p,
                        previous,
                        new SpellParams(party.Default, p),
                        PageUtil.GetOutOfBattlePlayableHandler(p))
                        .List;
            };
        }
    }
}