using Scripts.Model.Pages;
using System;

namespace Scripts.Model.World.Utility {

    /// <summary>
    /// Represents one encounter stored in a PageGenerator
    /// </summary>
    public class Encounter {
        public Func<Page> page;
        public Func<float> weight; // Chance of getting this encounter

        public Encounter(Func<Page> page, Func<float> weight) {
            this.page = page;
            this.weight = weight;
        }
    }
}