using Scripts.Model.Pages;
using System;
using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using System.Collections.Generic;

namespace Scripts.Game.Dungeons {

    /// <summary>
    /// Stages are groups of enemy encounters.
    /// </summary>
    /// <seealso cref="Scripts.Game.Dungeons.IStageable" />
    public class Stage : IStageable {

        /// <summary>
        /// The stage name
        /// </summary>
        public readonly string StageName;
        /// <summary>
        /// The encounters
        /// </summary>
        public readonly Func<Encounter[]> Encounters;

        /// <summary>
        /// Initializes a new instance of the <see cref="Stage"/> class.
        /// </summary>
        /// <param name="stageName">Name of the stage.</param>
        /// <param name="encounters">The encounters.</param>
        public Stage(string stageName, Func<Encounter[]> encounters) {
            this.StageName = stageName;
            this.Encounters = encounters;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stage"/> class.
        /// </summary>
        public Stage() {
            this.StageName = "Placeholder";
            this.Encounters = () => new Encounter[] { new Encounter(Music.NORMAL) };
        }

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
        Dungeon IStageable.GetStageDungeon(int dungeonIndex, int areaDungeonCount, Flags flags, IEnumerable<Character> party, Page camp, Page quests, AreaType area) {
            return new Dungeon(
                    party,
                    camp,
                    quests,
                    camp,
                    StageName,
                    string.Format("Selected stage {0} of {1}:\n{2}.\n\nEnter?",
                        (int)area,
                        area.GetDescription(),
                        StageName
                        ),
                    Encounters,
                    () => {
                        if (!flags.IsStageCleared(dungeonIndex, area)) {

                            // Not the last stage in an area
                            if (dungeonIndex < areaDungeonCount) {
                                flags.LastClearedStage++;

                                // is the last stage in an area
                            } else {
                                flags.LastClearedArea = area;
                                flags.LastClearedStage = 0;
                            }
                        }
                    }
                    );
        }
    }
}
