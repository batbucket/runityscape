using Scripts.Game.Dungeons;
using Scripts.Game.Serialized;
using Scripts.Game.Stages;
using Scripts.Model.Acts;
using Scripts.Model.Characters;
using Scripts.Model.Pages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Scripts.Game.Areas {

    /// <summary>
    /// Represents a single area in the game.
    /// Areas consist of intro and outro scenes, and
    /// have stages and places.
    /// </summary>
    public struct Area {

        /// <summary>
        /// We use this for serialization!
        /// </summary>
        public readonly AreaType Type;

        private int currentDungeonCount;
        private Stage[] stages;
        private PageGroup[] places;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="flags">Current game's flags</param>
        /// <param name="party">Current game's party</param>
        /// <param name="camp">Reference to the camp page, go here on victory and defeat</param>
        /// <param name="quests">Reference to quest page, go where if one wants to back out of entering</param>
        /// <param name="type">Type of area. Should be unique.</param>
        /// <param name="stages">List of stages to put into the area.</param>
        public Area(AreaType type, Stage[] stages, PageGroup[] places) {
            this.Type = type;
            this.currentDungeonCount = 0;
            this.stages = stages;
            this.places = places;
        }

        public Area(AreaType type, Stage[] stages) : this(type, stages, new PageGroup[0]) {
        }

        public ReadOnlyCollection<Stage> Stages {
            get {
                return new ReadOnlyCollection<Stage>(stages);
            }
        }

        public ReadOnlyCollection<PageGroup> Places {
            get {
                return new ReadOnlyCollection<PageGroup>(places);
            }
        }
    }
}