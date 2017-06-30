using Scripts.Game.Serialized;
using Scripts.Model.Pages;
using System;

namespace Scripts.Game.Pages {
    public class Encounter {
        public int Weight;
        public Func<Page> GeneratePage;
        public Func<Flags, bool> IsOverride; // True = always will occur
        public Func<Flags, bool> CanOccur; // False = will never occur

        /// <summary>
        /// Blueprint for an encounter
        /// </summary>
        /// <param name="weight">Chance of occurring in relation to other encounters</param>
        /// <param name="generatePage">Creates the encounter page</param>
        /// <param name="isOverride">If true, will always pick this encounter</param>
        /// <param name="canOccur">If false, will never pick this encounter</param>
        public Encounter(Rarity rarity, Func<Page> generatePage, Func<Flags, bool> isOverride, Func<Flags, bool> canOccur) {
            this.Weight = rarity.Weight;
            this.GeneratePage = generatePage;
            this.IsOverride = isOverride;
            this.CanOccur = canOccur;
        }

        public Encounter(Rarity rarity, Func<Page> generatePage, Func<Flags, bool> canOccur) : this(rarity, generatePage, (f) => false, canOccur) { }

        public Encounter(Rarity rarity, Func<Page> generatePage) : this(rarity, generatePage, (f) => false, (f) => true) { }
    }
}