using Scripts.Game.Dungeons;
using Scripts.Game.Serialized;
using Scripts.Model.Acts;
using Scripts.Model.Characters;
using Scripts.Model.Pages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Pages {
    public class Area {
        public readonly Act[] Intro;
        public readonly Act[] Outro;
        public readonly Dungeon[] Dungeons;
        public readonly IList<PageGroup> Places;
        public readonly AreaType Type;

        private readonly Flags flags;
        private readonly Music normal;
        private readonly Music boss;
        private readonly string areaName;
        private readonly Party party;
        private readonly Page camp;
        private readonly Page quests;
        private int currentDungeonCount;

        public Area(int dungeonCount, Flags flags, Party party, Page camp, Page quests, Music normal, Music boss, AreaType type, string areaName) {
            this.flags = flags;
            this.party = party;
            this.camp = camp;
            this.quests = quests;

            this.currentDungeonCount = 0;

            this.normal = normal;
            this.boss = boss;
            this.areaName = areaName;
            this.Intro = new Act[0];
            this.Outro = new Act[0];
            this.Dungeons = new Dungeon[dungeonCount];
            this.Places = new List<PageGroup>();
        }

        public void AddNormalStage(string stageName, Func<Encounter[]> encounters) {
            AddStage(stageName, normal, encounters);
        }

        public void AddBossStage(string stageName, Func<Encounter[]> encounters) {
            AddStage(stageName, boss, encounters);
        }

        private bool IsStageCleared(int stageIndex) {
            return flags.LastClearedArea > this.Type || flags.LastClearedStage > stageIndex;
        }

        private void AddStage(string stageName, Music music, Func<Encounter[]> encounters) {
            this.Dungeons[currentDungeonCount] =
                new Dungeon(
                    party, 
                    camp, 
                    quests,
                    camp,
                    music, 
                    string.Format("{0}-{1}",
                    stageName, 
                    currentDungeonCount), 
                    "Enter this stage?", 
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
