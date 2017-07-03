using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Scripts.Game.Serialized {
    public enum TimeOfDay {
        [Description("<color=white>Dawn</color>")]
        DAWN,
        [Description("<color=yellow>Morning</color>")]
        MORNING,
        [Description("<color=orange>Midday</color>")]
        MIDDAY,
        [Description("<color=red>Afternoon</color>")]
        AFTERNOON,
        [Description("<color=magenta>Dusk</color>")]
        DUSK,
        [Description("<color=blue>Night</color>")]
        NIGHT,
    }

    public enum Explore {
        RUINS
    }

    [Serializable]
    public class Flags {
        public TimeOfDay Time;
        public int DayCount;
        public List<Explore> UnlockedExplores;
        public int TotalExploreCount;
        public bool ShouldAdvanceTimeInCamp;

        public Flags() {
            this.UnlockedExplores = new List<Explore>();
        }
    }
}