namespace Scripts.Model.World.Utility {

    /// <summary>
    /// Stores event flags for the game.
    /// </summary>
    public class EventFlags {
        private const int MAX_LENGTH = 50;

        private bool[] bools;
        private int[] ints;

        public EventFlags() {
            this.ints = new int[MAX_LENGTH];
            this.bools = new bool[MAX_LENGTH];
        }

        public EventFlags(int[] ints, bool[] bools) {
            this.ints = ints;
            this.bools = bools;
        }

        public bool[] Bools {
            get {
                return bools;
            }
        }

        public int[] Ints {
            get {
                return ints;
            }
        }
    }
}