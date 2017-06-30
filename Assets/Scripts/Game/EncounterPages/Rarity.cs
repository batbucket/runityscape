namespace Scripts.Game.Pages {
    public sealed class Rarity {
        public static Rarity COMMON = new Rarity(100);

        public static Rarity UNCOMMON = new Rarity(50);
        public static Rarity VERY_UNCOMMON = new Rarity(25);

        public static Rarity RARE = new Rarity(12);
        public static Rarity VERY_RARE = new Rarity(6);

        public static Rarity IMPOSSIBLE = new Rarity(3);
        public static Rarity VERY_IMPOSSIBLE = new Rarity(1);

        private int weight;

        private Rarity(int weight) {
            this.weight = weight;
        }

        public int Weight {
            get {
                return weight;
            }
        }
    }
}