using Scripts.Model.Pages;
using Scripts.Model.Spells;

namespace Scripts.Game.Pages {
    public class EquipmentPages : PageGroup {

        public EquipmentPages(Page previous, SpellParams character) : base(new Page("Equipment")) {
            Setup(previous, character);
        }

        private void Setup(Page previous, SpellParams character) {
            Page p = Get(ROOT_INDEX);
            p.Icon = PageUtil.EQUIPMENT;
            p.OnEnter = () => {
                p.Actions = PageUtil.GenerateEquipmentGrid(
                    previous,
                    character,
                    PageUtil.GetOutOfBattlePlayableHandler(p)
                    ).List;
            };
        }
    }
}