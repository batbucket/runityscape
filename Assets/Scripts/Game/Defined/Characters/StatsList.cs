using Scripts.Game.Defined.Serialized.Statistics;
using Scripts.Model.Characters;
using Scripts.Model.Stats;

namespace Scripts.Game.Defined.StartingStats {
    public class KitsuneStats : Stats {
        public KitsuneStats() : base() {
            InitializeStats(5, 55, 5, 5, 1);
        }
    }

    public class HeroStats : Stats {
        public HeroStats() : base() {
            InitializeStats(0, 5, 5, 5, 5);
            AddStat(new Experience());
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
