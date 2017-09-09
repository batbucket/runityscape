using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using Scripts.Model.Pages;
using System.Collections.Generic;

namespace Scripts.Game.Dungeons {
    public interface IStageable {
        /// <summary>
        /// Gets the stage dungeon from this class.
        /// </summary>
        /// <param name="dungeonIndex">Index of the dungeon in the area.</param>
        /// <param name="areaDungeonCount">The number of dungeons in the area.</param>
        /// <param name="flags">Game flags.</param>
        /// <param name="party">Traversing party.</param>
        /// <param name="camp">Camp reference.</param>
        /// <param name="quests">Quest page reference.</param>
        /// <param name="area">The area.</param>
        /// <returns></returns>
        Dungeon GetStageDungeon(int dungeonIndex, int areaDungeonCount, Flags flags, IEnumerable<Character> party, Page camp, Page quests, AreaType area);
    }
}