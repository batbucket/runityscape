using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using Scripts.Model.Pages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Pages.Explorables {
    public class Ruins : ExplorableArea {
        public Ruins(
            Page camp,
            Flags flags,
            Party party,
            string location,
            Sprite sprite,
            string description
            )
            : base(
                  Explore.RUINS,
                  camp,
                  flags,
                  party,
                  "Ruins",
                  Util.GetSprite("castle-ruins"),
                  "The remains of some town visible from the campsite.") { }

        protected override IEnumerable<Encounter> GetEncounters() {
            return new Encounter[] {
                RandomBattle(Rarity.COMMON,null)
            };
        }
    }
}