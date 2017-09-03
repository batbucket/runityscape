using Scripts.Game.Dungeons;
using Scripts.Game.Pages;
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
        public AreaType CurrentArea;
        public AreaType LastClearedArea;
        public int LastClearedStage;
        public bool ShouldAdvanceTimeInCamp;

        public Flags() {
            this.LastClearedArea = AreaType.NONE;
            this.CurrentArea = AreaType.FIELD;
        }

        public FlagsSave GetSaveObject() {
            return new FlagsSave(this);
        }

        public void InitFromSaveObject(FlagsSave saveObject) {

        }
    }
}