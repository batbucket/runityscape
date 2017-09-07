using Scripts.Game.Dungeons;
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

            foreach (PageGroup pg in GetCurrentArea(flags.CurrentArea).Places) {
                buttons.Add(pg);
            }

            p.Actions = buttons;
        }

        private Process GetPlaceProcess(PageGroup pg) {
            return new Process(
                    pg.Root.Location,
                    pg.Root.TooltipText,
                    () => {
                        pg.Root.Invoke();
                        flags.ShouldAdvanceTimeInCamp = true;
                    }
                );
        }

        private Area GetCurrentArea(AreaType type) {
            return AreaList.ALL_AREAS[type](flags, party, previous, Root);
        }
    }
}