using Scripts.Model.Pages;
using System;
using Scripts.Game.Serialized;
using Scripts.Model.Characters;
using System.Collections.Generic;

namespace Scripts.Game.Dungeons {
    public class Stage : IStageable {
        public readonly string StageName;
        public readonly Func<Encounter[]> Encounters;

        public Stage(string stageName, Func<Encounter[]> encounters) {
            this.StageName = stageName;
            this.Encounters = encounters;
        }

        public Stage() {
            this.StageName = "Placeholder";
            this.Encounters = () => new Encounter[] { new Encounter(Music.NORMAL) };
        }

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
