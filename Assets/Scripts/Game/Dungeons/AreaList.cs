using Scripts.Game.Defined.Characters;
using Scripts.Game.Pages;
using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using Scripts.Model.Pages;
using System;
using System.Collections.Generic;

namespace Scripts.Game.Dungeons {
    public static class AreaList {
        private const int DUNGEON_COUNT = 8;

        /// <summary>
        /// Flags = World flags for current save
        /// Party = current party
        /// Page = Camp reference
        /// Page = DungeonPages reference
        /// Area = Area to return
        /// </summary>
        public static readonly ReadOnlyDictionary<AreaType, Func<Flags, Party, Page, Page, Area>> ALL_AREAS 
            = new ReadOnlyDictionary<AreaType, Func<Flags, Party, Page, Page, Area>>(
                new Dictionary<AreaType, Func<Flags, Party, Page, Page, Area>>() {
                    { AreaType.FIELD, (f, p, c, q) => CreateField(f, p, c, q) }
        });

        private static Area CreateArea(AreaType type, Flags flags, Party party, Page camp, Page quests) {
            Area area = new Area(DUNGEON_COUNT, flags, party, camp, quests, type);
            return area;
        }

        private static Area CreateField(Flags flags, Party party, Page camp, Page quests) {
            return CreateArea(AreaType.FIELD, flags, party, camp, quests)
                .AddStage(
                    "Zero Prairie",
                    () => new Encounter[] {
                        new Encounter(Music.NORMAL, CharacterList.Ruins.Villager()),
                        new Encounter(Music.NORMAL, CharacterList.Ruins.Villager(), CharacterList.Ruins.Villager())
                    })
                .AddStage(
                    "One Prairie",
                    () => new Encounter[] {
                        new Encounter(Music.NORMAL, CharacterList.Ruins.Villager()),
                        new Encounter(Music.NORMAL, CharacterList.Ruins.Villager(), CharacterList.Ruins.Villager())
                    }
                );
        }
    }
}