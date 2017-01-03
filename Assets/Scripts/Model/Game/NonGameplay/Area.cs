namespace Scripts.Model {

    /// <summary>
    /// Represents a group of related pages in the game.
    /// </summary>
    public abstract class Area {

        public Area() {
            Init();
        }

        /// <summary>
        /// Post constructor intitialization.
        /// </summary>
        public abstract void Init();
    }
}