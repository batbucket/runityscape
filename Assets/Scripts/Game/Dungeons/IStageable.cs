using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using Scripts.Model.Pages;

namespace Scripts.Game.Dungeons {
    public interface IStageable {
        Dungeon GetStageDungeon(int dungeonIndex, int areaDungeonCount, Flags flags, Party party, Page camp, Page quests, AreaType area);
    }
}