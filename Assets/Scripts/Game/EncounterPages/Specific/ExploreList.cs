using Scripts.Game.Defined.Characters;
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
            Party party
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
                RandomBattle(Rarity.COMMON, CharacterList.Ruins.Villager())
                    .AddOverride(flags => flags.TotalExploreCount < 2),
                RandomBattle(Rarity.UNCOMMON, CharacterList.Ruins.Knight()),
                RandomBattle(Rarity.UNCOMMON, CharacterList.Ruins.Healer()),
                RandomBattle(Rarity.RARE, CharacterList.Ruins.Knight(), CharacterList.Ruins.Healer()),
                RandomBattle(Rarity.VERY_RARE, CharacterList.Ruins.Knight(), CharacterList.Ruins.Knight(), CharacterList.Ruins.Healer()),
                RandomBattle(Rarity.VERY_RARE, CharacterList.Ruins.Villager(), CharacterList.Ruins.Villager(), CharacterList.Ruins.Villager(), CharacterList.Ruins.Villager(), CharacterList.Ruins.Villager())
            };
        }
    }
}