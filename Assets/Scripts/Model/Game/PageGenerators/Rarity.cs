namespace Scripts.Model.World.PageGenerators {

    /// <summary>
    /// Describes how common it is to get an encounter from a PageGenerator
    /// </summary>
    public sealed class Rarity {
        public static readonly Rarity COMMON = new Rarity(1);
        public static readonly Rarity UNCOMMON = new Rarity(0.5f);
        public static readonly Rarity RARE = new Rarity(0.25f);
        public static readonly Rarity SUPER_RARE = new Rarity(0.10f);
        public static readonly Rarity IMPOSSIBLE = new Rarity(0.01f);

        private float weight;
        private Rarity(float weight) {
            this.weight = weight;
        }

        public float Weight {
            get {
                return weight;
            }
        }
    }
}