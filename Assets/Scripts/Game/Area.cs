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

        public Area(Flags flags, Party party, Page camp, Page quests, AreaType type, params IStageable[] stagables) {
            this.flags = flags;
            this.party = party;
            this.camp = camp;
            this.quests = quests;
            this.Type = type;

            this.currentDungeonCount = 0;

            this.areaName = type.GetDescription();
            this.Intro = new Act[0];
            this.Outro = new Act[0];
            this.Dungeons = new Dungeon[stagables.Length];
            this.Places = new List<PageGroup>();

            for (int i = 0; i < stagables.Length; i++) {
                AddStage(stagables[i], stagables.Length);
            }
        }

        public static bool IsStageCleared(int stageIndex, AreaType type, Flags flags) {
            return flags.LastClearedArea > type || flags.LastClearedStage > stageIndex;
        }

        private void AddStage(IStageable stagable, int totalNumberOfStages) {

            this.Dungeons[currentDungeonCount] = stagable.GetStageDungeon(currentDungeonCount, totalNumberOfStages, flags, party, camp, quests, this.Type);
            currentDungeonCount++;
        }
    }
}
