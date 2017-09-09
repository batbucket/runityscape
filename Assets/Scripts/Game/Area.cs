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

    /// <summary>
    /// Represents a single area in the game.
    /// Areas consist of intro and outro scenes, and
    /// have stages and places.
    /// </summary>
    public class Area {

        /// <summary>
        /// Intro scene, played once when entering the area.
        /// </summary>
        public readonly Act[] Intro;

        /// <summary>
        /// Outro scene, played once when the area's boss is defeated.
        /// </summary>
        public readonly Act[] Outro;

        /// <summary>
        /// List of stages for the area.
        /// </summary>
        public readonly Dungeon[] Stages;

        /// <summary>
        /// Extra areas, such as shops, other dungeons, and etc.
        /// </summary>
        public readonly IList<PageGroup> Places;

        /// <summary>
        /// We use this for serialization!
        /// </summary>
        public readonly AreaType Type;

        private readonly Flags flags;
        private readonly string areaName;
        private readonly Party party;
        private readonly Page camp;
        private readonly Page quests;
        private int currentDungeonCount;

        /// <summary>
        ///
        /// </summary>
        /// <param name="flags">Current game's flags</param>
        /// <param name="party">Current game's party</param>
        /// <param name="camp">Reference to the camp page, go here on victory and defeat</param>
        /// <param name="quests">Reference to quest page, go where if one wants to back out of entering</param>
        /// <param name="type">Type of area. Should be unique.</param>
        /// <param name="stagables">List of stageable objects that can be converted into stages.</param>
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
            this.Stages = new Dungeon[stagables.Length];
            this.Places = new List<PageGroup>();

            for (int i = 0; i < stagables.Length; i++) {
                AddStage(stagables[i], stagables.Length);
            }
        }

        private void AddStage(IStageable stagable, int totalNumberOfStages) {
            this.Stages[currentDungeonCount] = stagable.GetStageDungeon(currentDungeonCount, totalNumberOfStages, flags, party, camp, quests, this.Type);
            currentDungeonCount++;
        }
    }
}
