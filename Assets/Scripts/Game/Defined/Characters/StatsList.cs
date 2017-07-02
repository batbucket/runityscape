using Scripts.Game.Defined.Serialized.Statistics;
using Scripts.Model.Characters;
using Scripts.Model.Stats;

namespace Scripts.Game.Defined.StartingStats {
    public class KitsuneStats : Stats {
        public KitsuneStats() : base() {
            InitializeStats(5, 55, 1, 5, 1);
        }
    }

    public class HeroStats : Stats {
        public HeroStats() : base() {
            InitializeStats(0, 5, 5, 5, 5);
            AddStat(new Experience());
        }
    }

    public class VillagerStats : Stats {
        public VillagerStats() {
            InitializeStats(1, 1, 1, 1, 1);
        }
    }

    public class KnightStats : Stats {
        public KnightStats() {
            InitializeStats(2, 5, 5, 5, 2);
        }
    }

    public class HealerStats : Stats {
        public HealerStats() {
            InitializeStats(4, 1, 10, 10, 3);
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
