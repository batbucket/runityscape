using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.Model.Spells;

namespace Scripts.Game.Pages {
    public class EquipmentPages : PageGroup {

        public EquipmentPages(Page previous, Character character) : base(new Page("Equipment")) {
            Setup(previous, character);
        }

        private void Setup(Page previous, Character character) {
            Page p = Get(ROOT_INDEX);
            p.AddCharacters(Side.LEFT, character);
            p.Icon = PageUtil.EQUIPMENT;
            p.OnEnter = () => {
                p.Actions = PageUtil.GenerateEquipmentGrid(
                    previous,
                    new SpellParams(character, p),
                    PageUtil.GetOutOfBattlePlayableHandler(p)
                    ).List;
            };
        }
    }
}