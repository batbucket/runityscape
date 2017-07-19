using System;
using Scripts.Model.Characters;
using Scripts.Model.Pages;
using Scripts.Game.Defined.Characters;
using Scripts.Game.Serialized;

namespace Scripts.Game.Pages {
    public class CathedralDungeon : Dungeon {
        public CathedralDungeon(Flags flags, Party party, Page defeat, Page previous, Page destination)
            : base(party,
                  defeat,
                  previous,
                  destination,
                  Music.CATHEDRAL,
                  "Halls",
                  "As you stand under the archway leading into the cathedral, you hear countless ghostly screams coming from the depths of the place."
                  ) {
            Root.AddCharacters(Side.LEFT, party);
            Root.Condition = () => (flags.Ruins == RuinsProgression.ASK_ABOUT_FOX);
        }

        protected override Character[][] GetEnemyEncounters() {
            return new Character[][] {
                new Character[] { CharacterList.Ruins.Villager(), CharacterList.Ruins.Villager() },
                new Character[] { CharacterList.Ruins.Knight() },
                new Character[] { CharacterList.Ruins.Healer(), CharacterList.Ruins.Healer() },
                new Character[] { CharacterList.Ruins.Knight(), CharacterList.Ruins.Healer() }
            };
        }
    }
}