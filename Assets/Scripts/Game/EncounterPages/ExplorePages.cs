using Scripts.Game.Serialized;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Pages {
    public class ExplorePages : PageGroup {
        private readonly IDictionary<Explore, PageGenerator> explores = new Dictionary<Explore, PageGenerator>();

        public ExplorePages(Page previous, Flags flags, ref bool shouldAdvanceTime) : base(new Page("Explore")) {
            var buttons = new List<IButtonable>();

            SetupGenerators(Root, flags);

            foreach (KeyValuePair<Explore, PageGenerator> pair in explores) {
                if (flags.UnlockedExplores.Contains(pair.Key)) {
                    buttons.Add(GetEncounterProcess(flags, pair.Value));
                }
            }

            Get(ROOT_INDEX).Actions = buttons;
        }

        private Process GetEncounterProcess(Flags flags, PageGenerator gen) {
            return new Process(
                gen.Name,
                gen.Sprite,
                string.Format("Explore this area.\n{0}", gen.Description),
                () => gen.GetRandomEncounter(flags).Invoke()
                );
        }

        private void SetupGenerators(Page previous, Flags flags) {
            explores.Add(Explore.RUINS, Ruins(previous, flags));
        }

        private PageGenerator Ruins(Page previous, Flags flags) {
            PageGenerator pg = new PageGenerator("Ruins", Util.GetSprite("tentacle"), "A derelict town visible from the campsite.");
            return pg;
            //pg.AddEncounter(new Encounter(Rarity.COMMON);
        }
    }
}