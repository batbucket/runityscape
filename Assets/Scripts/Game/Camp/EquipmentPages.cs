using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.Model.Spells;

namespace Scripts.Game.Pages {

    /// <summary>
    /// Pages for equipping items out of battle.
    /// </summary>
    /// <seealso cref="Scripts.Model.Pages.PageGroup" />
    public class EquipmentPages : PageGroup {

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentPages"/> class.
        /// </summary>
        /// <param name="previous">The previous.</param>
        /// <param name="party">The party.</param>
        public EquipmentPages(Page previous, Party party) : base(new Page("Equipment")) {
            Setup(previous, party);
        }

        private void Setup(Page previous, Party party) {
            Page p = Get(ROOT_INDEX);
            p.AddCharacters(Side.LEFT, party);
            p.Icon = PageUtil.EQUIPMENT;
            p.OnEnter = () => {
                p.Actions = PageUtil.GenerateGroupEquipmentGrid(
                    previous,
                    p,
                    party.Collection,
                    PageUtil.GetOutOfBattlePlayableHandler(p)
                    ).List;
            };
        }
    }
}