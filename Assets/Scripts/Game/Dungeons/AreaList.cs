using Scripts.Game.Pages;
using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using Scripts.Model.Pages;

namespace Scripts.Game.Dungeons {
    public static class AreaList {
        private const int DUNGEON_COUNT = 8;

        public Area CreateField(Flags flags, Party party, Page camp, Page quests) {
            Area area = new Area(DUNGEON_COUNT, flags, party, camp, quests, Music.NORMAL, Music.BOSS, AreaType.FIELD, "Field");
        }
    }
}