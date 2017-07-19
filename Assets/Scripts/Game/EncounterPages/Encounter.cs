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
        public Encounter(Rarity rarity, Func<Page> generatePage) {
            this.Weight = rarity.Weight;
            this.GeneratePage = generatePage;
            this.IsOverride = f => false;
            this.CanOccur = f => true;
        }

        public Encounter AddOverride(Func<Flags, bool> isOverride) {
            IsOverride = isOverride;
            return this;
        }

        public Encounter AddEnable(Func<Flags, bool> isEnabled) {
            this.CanOccur = isEnabled;
            return this;
        }
    }
}