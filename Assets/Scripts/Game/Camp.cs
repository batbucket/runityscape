
using Scripts.Game.Defined.Characters;
using Scripts.Game.Defined.Serialized.Statistics;
using Scripts.Game.Serialized;
using Scripts.Game.Undefined.Characters;
using Scripts.Model.Acts;
using Scripts.Model.Characters;
using Scripts.Model.Interfaces;
using Scripts.Model.Pages;
using Scripts.Model.Processes;
using Scripts.Model.Spells;
using Scripts.Model.Stats;
using Scripts.Model.TextBoxes;
using Scripts.Presenter;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts.Game.Pages {

    public class Camp : PageGroup {
        private Flags flags;
        private Party party;

        public Camp(Party party, Flags flags) : base(new Page("Campsite")) {
            this.party = party;
            this.flags = flags;
            SetupCamp();
        }

        private void SetupCamp() {
            Page p = Get(ROOT_INDEX);
            p.OnEnter = () => {
                p.Left.Clear();
                p.Right.Clear();
                p.AddCharacters(Side.LEFT, party.Collection);
                p.Actions = new IButtonable[] {
                new LevelUpPages(Root, party.Default),
                new InventoryPages(p, party.Default, party.shared),
                new EquipmentPages(p, new SpellParams(party.Default)),
                new SavePages(p, party, flags)
            };
            };
        }
    }

}