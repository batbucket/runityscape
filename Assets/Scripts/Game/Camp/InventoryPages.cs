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

    /// <summary>
    /// Inventory pages for managing items in camp.
    /// </summary>
    /// <seealso cref="Scripts.Model.Pages.PageGroup" />
    public class InventoryPages : PageGroup {

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryPages"/> class.
        /// </summary>
        /// <param name="previous">The previous page to back out to.</param>
        /// <param name="party">The party whose inventory needs to be managed.</param>
        public InventoryPages(Page previous, Party party) : base(new Page(string.Format("Inventory ({0})", party.Shared.Fraction))) {
            Inventory(previous, party, party.Shared);
        }

        private void Inventory(Page previous, Party party, Inventory inventory) {
            Page p = Get(ROOT_INDEX);
            p.AddCharacters(Side.LEFT, party);
            p.Icon = PageUtil.INVENTORY;
            p.OnEnter = () => {
                p.AddText(string.Format("{0}/{1} spaces used.", inventory.TotalOccupiedSpace, inventory.Capacity));
                p.Actions = PageUtil.GenerateGroupItemsGrid(
                        p,
                        previous,
                        party,
                        PageUtil.GetOutOfBattlePlayableHandler(p))
                        .List;
            };
        }
    }
}