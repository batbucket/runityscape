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
            InitializeStats(0, 1, 1, 1, 5);
            AddStat(new Experience());
        }
    }

    public class VillagerStats : Stats {
        public VillagerStats() {
            InitializeStats(2, 1, 1, 1, 2);
        }
    }

    public class KnightStats : Stats {
        public KnightStats() {
            InitializeStats(3, 2, 2, 2, 4);
        }
    }

    public class HealerStats : Stats {
        public HealerStats() {
            InitializeStats(3, 1, 5, 5, 1);
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
