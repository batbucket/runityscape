using Scripts.Game.Defined.Serialized.Statistics;
using Scripts.Model.Characters;
using Scripts.Model.Stats;

namespace Scripts.Game.Defined.StartingStats {
    public static class StatsList {

        public static Stats Hero() {
            Stats st = new Stats(0, 1, 1, 1, 5);
            st.AddStat(new Experience());
            return st;
        }
    }
}

namespace Scripts.Game.Undefined.StartingStats {
    public class DummyStats : Stats {
        public DummyStats(int level) : base() {
            InitializeStats(level, 1, 1, 1, 10);
            RemoveStat(StatType.HEALTH);
        }
    }
}
