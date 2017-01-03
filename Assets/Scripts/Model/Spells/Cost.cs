using Scripts.Model.Stats.Resources;

namespace Scripts.Model.Spells {

    /// <summary>
    /// Helper struct to for passing parameters.
    /// </summary>
    public struct Cost {
        public ResourceType resource;
        public int amount;

        public Cost(ResourceType res, int amount) {
            this.resource = res;
            this.amount = amount;
        }
    }
}