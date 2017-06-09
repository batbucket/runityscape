using Scripts.Model.Stats;

namespace Scripts.Model.Characters {
    public static class StatList {
        public class Kitsune : Stats {
            public Kitsune() : base() {
                InitializeStats(5, 5, 5, 5, 1);
            }
        }
    }
}
