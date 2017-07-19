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

    public enum Explore {
        RUINS
    }

    public enum Place {
        CATHEDRAL
    }

    public enum RuinsProgression {
        UNEXPLORED,
        FIRST_VISIT,
        ENCOUNTER_MAPLE,
        ASK_ABOUT_INJURIES,
        ASK_ABOUT_FOX,
        BOSS_CONFRONTED,
        BOSS_VICTORY
    }

    [Serializable]
    public class Flags : ISaveable<FlagsSave> {
        public TimeOfDay Time;
        public int DayCount;
        public int TotalExploreCount;
        public bool ShouldAdvanceTimeInCamp;
        public RuinsProgression Ruins;

        [NonSerialized]
        private HashSet<Explore> unlockedExplores;
        [NonSerialized]
        private HashSet<Place> unlockedPlaces;

        public Flags() {
            this.unlockedExplores = new HashSet<Explore>();
            this.unlockedPlaces = new HashSet<Place>();
        }

        public ICollection<Explore> UnlockedExplores {
            get {
                return unlockedExplores;
            }
        }

        public ICollection<Place> UnlockedPlaces {
            get {
                return unlockedPlaces;
            }
        }

        public FlagsSave GetSaveObject() {
            return new FlagsSave(this);
        }

        public void InitFromSaveObject(FlagsSave saveObject) {
            this.unlockedExplores = new HashSet<Explore>(saveObject.Explores);
            this.unlockedPlaces = new HashSet<Place>(saveObject.Places);
        }
    }
}