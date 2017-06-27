
using Scripts.Game.Defined.Characters;
using Scripts.Game.Defined.Serialized.Statistics;
using Scripts.Game.Undefined.Characters;
using Scripts.Model.Acts;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.Stats;
using Scripts.Model.TextBoxes;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts.Game.Pages {

    public class Camp : PageGroup {
        private Party party;
        private const int CHARACTER = 1;

        public Camp(string name) : base(new Page("Campsite")) {
            this.party = new Party();
            party.AddMember(new Hero(name));
            Register(CHARACTER, new Page(party.Default.Look.DisplayName));
            SetupCamp();
        }

        public Camp(Party party) : this(party.Default.Look.DisplayName) {
            this.party = party;
        }

        private void SetupCamp() {
            Page p = Get(ROOT_INDEX);
            p.Actions = new IButtonable[] {
                new LevelUp(Root, party.Default)
            };
            p.AddCharacters(Side.LEFT, party.Collection);
        }

        private Page CampOption(int index, List<IButtonable> buttons) {
            Page p = BasicPage(index, ROOT_INDEX, buttons.ToArray());
            p.AddCharacters(Side.LEFT, party.Collection);
            return p;
        }
    }

}