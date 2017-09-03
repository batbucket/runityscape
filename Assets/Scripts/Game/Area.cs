using Scripts.Game.Dungeons;
using Scripts.Game.Serialized;
using Scripts.Model.Acts;
using Scripts.Model.Characters;
using Scripts.Model.Pages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Dungeons {
    public class Area {
        public readonly Act[] Intro;
        public readonly Act[] Outro;
        public readonly Dungeon[] Dungeons;
        public readonly IList<PageGroup> Places;
        public readonly AreaType Type;

        private readonly Flags flags;
        private readonly string areaName;
        private readonly Party party;
        private readonly Page camp;
        private readonly Page quests;
        private int currentDungeonCount;

        public Area(Flags flags, Party party, Page camp, Page quests, AreaType type, params Stage[] stages) {
            this.flags = flags;
            this.party = party;
            this.camp = camp;
            this.quests = quests;
            this.Type = type;

            this.currentDungeonCount = 0;

            this.areaName = type.GetDescription();
            this.Intro = new Act[0];
            this.Outro = new Act[0];
            this.Dungeons = new Dungeon[stages.Length];
            this.Places = new List<PageGroup>();

            for (int i = 0; i < stages.Length; i++) {
                AddStage(stages[i]);
            }
        }

        public bool IsStageCleared(int stageIndex) {
            return flags.LastClearedArea > this.Type || flags.LastClearedStage > stageIndex;
        }

        private void AddStage(Stage stage) {
            string stageName = stage.StageName;
            Func<Encounter[]> encounters = stage.Encounters;

            this.Dungeons[currentDungeonCount] =
                new Dungeon(
                    party, 
                    camp, 
                    quests,
                    camp,
                    stageName, 
                    string.Format("Selected stage {0} of {1}:\n{2}.\n\nEnter?", 
                        (int)Type, 
                        Type.GetDescription(), 
                        stageName
                        ), 
                    encounters,
                    () => {
                        if (!IsStageCleared(currentDungeonCount)) {

                            // Not the last stage in an area
                            if (currentDungeonCount < Dungeons.Length) {
                                flags.LastClearedStage++;

                            // is the last stage in an area
                            } else {
                                flags.LastClearedArea = this.Type;
                                flags.LastClearedStage = 0;
                            }
                        }
                    }
                    );
            currentDungeonCount++;
        }
    }
}
