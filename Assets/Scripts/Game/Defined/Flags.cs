using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;
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

    [Serializable]
    public class Flags : ISaveable<FlagsSave> {
        public TimeOfDay Time;
        public int DayCount;
        public int TotalExploreCount;
        public bool ShouldAdvanceTimeInCamp;

        public Flags() {

        }

        public FlagsSave GetSaveObject() {
            return new FlagsSave(this);
        }

        public void InitFromSaveObject(FlagsSave saveObject) {

        }
    }
}