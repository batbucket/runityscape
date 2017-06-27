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

        public InventoryPages(Page previous, Character c, Inventory inventory) : base(new Page("Inventory")) {
            Inventory(previous, c, inventory);
        }

        private void Inventory(Page previous, Character c, Inventory inventory) {
            Page p = Get(ROOT_INDEX);
            p.AddCharacters(Side.LEFT, c);
            p.OnEnter = () => {
                p.AddText(string.Format("{0}/{1} spaces used.", inventory.TotalOccupiedSpace, inventory.Capacity));
                p.Actions = PageUtil.GenerateItems(
                        p,
                        previous,
                        new SpellParams(c),
                        null,
                        inventory,
                        inventory.UniqueItemCount,
                        playable => {
                            Main.Instance.StartCoroutine(PerformInOrder(p, playable, () => p.OnEnter()));
                        })
                        .Array;
            };
        }

        /// <summary>
        /// There's a slight delay to Play()'s effects occurring, so we wait for
        /// Play() to be completely finished, and then perform our action.
        ///
        /// If we don't do this, then item counts won't be updated since it'll go in this order:
        /// Play coroutine started
        /// Item buttons get setup
        /// Used item is decremented in count by 1
        /// </summary>
        /// <param name="en">PlayGroup</param>
        /// <param name="a">Action to perform after PlayGroup is finished.</param>
        /// <returns></returns>
        private IEnumerator PerformInOrder(Page page, IPlayable play, Action postAction) {
            yield return play.Play();
            page.AddText(play.Text);
            postAction.Invoke();
        }
    }
}