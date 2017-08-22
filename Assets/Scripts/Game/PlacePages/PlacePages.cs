using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using System.Collections.Generic;

namespace Scripts.Game.Pages {
    public class PlacePages : PageGroup {
        private readonly Page previous;
        private readonly Flags flags;
        private readonly Party party;

        public PlacePages(Page previous, Flags flags, Party party) : base(new Page("Places")) {
            this.previous = previous;
            this.flags = flags;
            this.party = party;

            SetupRoot();
        }

        private void SetupRoot() {
            Page p = Root;
            p.Condition = PageUtil.GetVisitProcessCondition(flags, party);
            p.Icon = Util.GetSprite("walking-boot");
            p.Body = "Where would you like to go?";
            p.AddCharacters(Side.LEFT, party);
            var buttons = new List<IButtonable>();
            buttons.Add(PageUtil.GenerateBack(previous));

            p.Actions = buttons;
        }
    }
}