using Scripts.Game.Pages.Explorables;
using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Game.Pages {
    public class ExplorePages : PageGroup {
        private readonly IDictionary<Explore, ExplorableArea> explores = new Dictionary<Explore, ExplorableArea>();
        private readonly Party party;
        private readonly Flags flags;
        private readonly Page previous;

        public ExplorePages(Page previous, Party party, Flags flags) : base(new Page("Explore")) {
            var buttons = new List<IButtonable>();
            this.party = party;
            this.flags = flags;
            this.previous = previous;

            Root.AddCharacters(Side.LEFT, party);
            Root.Condition = () => flags.Time != TimeOfDay.NIGHT && party.Any(c => c.Stats.State == State.ALIVE);

            SetupGenerators();
            buttons.Add(PageUtil.GenerateBack(previous));

            foreach (KeyValuePair<Explore, ExplorableArea> pair in explores) {
                if (flags.UnlockedExplores.Contains(pair.Key)) {
                    buttons.Add(GetEncounterProcess(pair.Value.Generator));
                }
            }

            Get(ROOT_INDEX).Actions = buttons;
            Get(ROOT_INDEX).OnEnter = () => {
                Get(ROOT_INDEX)
                .AddText(
                    "Where would you like to explore?"
                    );
            };
        }

        private Process GetEncounterProcess(PageGenerator gen) {
            return new Process(
                gen.Name,
                gen.Sprite,
                string.Format("Explore this area.\n{0}", gen.Description),
                () => {
                    gen.GetRandomEncounter(flags).Invoke();
                    flags.TotalExploreCount++;
                    flags.ShouldAdvanceTimeInCamp = true;
                }
                );
        }

        private void AddArea(ExplorableArea area) {
            explores.Add(area.Type, area);
        }

        private void SetupGenerators() {
            flags.UnlockedExplores.Add(Explore.RUINS);
            AddArea(new Ruins(previous, flags, party));
        }
    }
}