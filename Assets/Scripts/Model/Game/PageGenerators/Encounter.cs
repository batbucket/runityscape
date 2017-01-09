using Scripts.Model.Pages;
using System;

namespace Scripts.Model.World.PageGenerators {

    /// <summary>
    /// Represents one encounter stored in a PageGenerator
    /// </summary>
    public class Encounter {

        /// <summary>
        /// Page to be generated if this encounter is selected.
        /// </summary>
        public Page Page { get { return this.pageFunc.Invoke(); } }

        /// <summary>
        /// Chance of getting this Encounter.
        /// </summary>
        public float Weight { get { return this.weight; } }

        /// <summary>
        /// If true, this encounter can occur.
        /// </summary>
        public bool IsEnabled { get { return this.isEnabledFunc.Invoke(); } }

        private Func<Page> pageFunc;
        private float weight;
        private Func<bool> isEnabledFunc;

        private Encounter(Func<Page> page, float weight, Func<bool> isEnabled = null) {
            this.pageFunc = page;
            this.weight = weight;
            this.isEnabledFunc = isEnabled ?? (() => { return true; });
        }

        public Encounter(Func<Page> page, Rarity rarity, Func<bool> isEnabled = null) : this(page, rarity.Weight, isEnabled) { }
    }
}