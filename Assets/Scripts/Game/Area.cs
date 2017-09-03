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

        public Area(int dungeonCount, Flags flags, Party party, Page camp, Page quests, AreaType type) {
            this.flags = flags;
            this.party = party;
            this.camp = camp;
            this.quests = quests;

            this.currentDungeonCount = 0;

            this.areaName = type.GetDescription();
            this.Intro = new Act[0];
            this.Outro = new Act[0];
            this.Dungeons = new Dungeon[dungeonCount];
            this.Places = new List<PageGroup>();
        }

        public bool IsStageCleared(int stageIndex) {
            return flags.LastClearedArea > this.Type || flags.LastClearedStage > stageIndex;
        }

        public Area AddStage(string stageName, Func<Encounter[]> encounters) {
            this.Dungeons[currentDungeonCount] =
                new Dungeon(
                    party, 
                    camp, 
                    quests,
                    camp,
                    stageName, 
                    string.Format("You have selected stage ({0}-{1}):\n{2}.\nEnter?", 
                        (int)Type, 
                        currentDungeonCount, 
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
            return this;
        }
    }
}
