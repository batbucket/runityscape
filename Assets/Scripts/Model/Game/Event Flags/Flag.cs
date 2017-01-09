namespace Scripts.Model.World.Flags {

    /// <summary>
    /// Stores indicies for easy Flags checking.
    /// </summary>
    public static class Flag {

        // Bools indicies
        public const int DISCOVERED_TEMPLE = 0;

        public const int SHOPKEEPER_GAVE_NAME = 1;
        public const int SHOPKEEPER_NO_SELL = 2;
        public const int SHOPKEEPER_MENTIONED_MURDER = 3;
        public const int SHOPKEEPER_MENTIONED_SISTER = 4;
        public const int SHOPKEEPER_MENTIONED_KEEPER = 5;
        public const int SHOPKEEPER_BOUGHT_SOMETHING = 6;

        // Ints indicies
        public const int DAYS = 0;

        public const int TIME = 1;
        public const int SHOPKEEPER_STATUS = 2;
        public const int TEMPLE_STATUS = 3;

        // Ints values
        public const int SHOPKEEPER_NEUTRAL = 0;

        public const int SHOPKEEPER_FRIENDLY = 1;
        public const int SHOPKEEPER_ENEMY = 2;
        public const int SHOPKEEPER_DEAD = 3;

        public const int TEMPLE_ENTRANCE_CLEARED = 1;
        public const int TEMPLE_BOSS_MET = 2;
        public const int TEMPLE_BOSS_CLEARED = 3;
    }
}