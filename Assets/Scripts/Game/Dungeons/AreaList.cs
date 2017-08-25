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

        public static ReadOnlyDictionary<AreaType, Area> AllAreas {
            get {
                return new ReadOnlyDictionary<AreaType, Area>(ALL_AREAS);
            }
        }

        private static readonly IDictionary<AreaType, Area> ALL_AREAS = new Dictionary<AreaType, Area>();

        private static Area CreateArea(AreaType type, Flags flags, Party party, Page camp, Page quests) {
            Area area = new Area(DUNGEON_COUNT, flags, party, camp, quests, type);
            ALL_AREAS.Add(area.Type, area);
            return area;
        }

        public static void CreateField(Flags flags, Party party, Page camp, Page quests) {
            CreateArea(AreaType.FIELD, flags, party, camp, quests)
                .AddStage(
                    "Initial Prairie",
                    () => new Encounter[] {
                    new Encounter(Music.NORMAL, CharacterList.Ruins.Villager()),
                    new Encounter(Music.NORMAL, CharacterList.Ruins.Villager(), CharacterList.Ruins.Villager())
                    })
                .AddStage(
                    "Initial Prairie II",
                    () => new Encounter[] {
                    new Encounter(Music.NORMAL, CharacterList.Ruins.Villager()),
                    new Encounter(Music.NORMAL, CharacterList.Ruins.Villager(), CharacterList.Ruins.Villager())
                    }
                );
        }
    }
}