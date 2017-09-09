using Scripts.Game.Serialized;
using Scripts.Model.Items;
using System;
using UnityEngine;

namespace Scripts.Game.Shopkeeper {

    /// <summary>
    /// Class to facilitate purchasing items from a shopkeeper.
    /// </summary>
    public class Buy {

        /// <summary>
        /// Returns a new item
        /// </summary>
        public readonly Func<Item> GetItem;

        /// <summary>
        /// Condition for item to appear
        /// </summary>
        public Func<Flags, bool> Condition;

        /// <summary>
        /// Flags to set after item is purchased
        /// </summary>
        public Action<Flags> SetFlags;

        /// <summary>
        /// Base price of the item
        /// </summary>
        public readonly int BasePrice;

        /// <summary>
        /// Name of the item
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Sales pitch of the item
        /// </summary>
        public string SalesPitch;

        /// <summary>
        /// Sprite of the item
        /// </summary>
        public readonly Sprite Sprite;

        private string description;

        /// <summary>
        /// Main
        /// </summary>
        /// <param name="itemToBuy">Item to be purchased by the player.</param>
        public Buy(Item itemToBuy) {
            GetItem = () => Util.TypeToObject<Item>(itemToBuy.GetType());
            this.Condition = f => true;
            this.SetFlags = f => { };
            this.BasePrice = itemToBuy.BasePrice;
            this.Name = itemToBuy.Name;
            this.description = itemToBuy.Description;
            this.Sprite = itemToBuy.Icon;
            this.SalesPitch = string.Empty;
        }

        /// <summary>
        /// Get the description.
        /// </summary>
        public string Description {
            get {
                return
                    string.Format("{0}{1}",
                    description,
                        string.IsNullOrEmpty(SalesPitch) ?
                        string.Empty :
                            string.Format("\n\n{0}",
                            SalesPitch));
            }
        }

        /// <summary>
        /// Add a sales pitch.
        /// </summary>
        /// <param name="salesPitch">Pitch</param>
        /// <returns>Buy object for method chaining</returns>
        public Buy AddPitch(string salesPitch) {
            SalesPitch = string.Format("\"{0}\"", salesPitch);
            return this;
        }

        /// <summary>
        /// Add a condition to the buy
        /// </summary>
        /// <param name="condition">If true, item can be bought</param>
        /// <returns>Buy object for method chaining</returns>
        public Buy AddCondition(Func<Flags, bool> condition) {
            this.Condition = condition;
            return this;
        }

        /// <summary>
        /// Add flag fields to be set on item purchase.
        /// </summary>
        /// <param name="setFlags">Action that causes flag fields to be set.</param>
        /// <returns>Buy object for method chaining</returns>
        public Buy AddSetFlags(Action<Flags> setFlags) {
            this.SetFlags = setFlags;
            return this;
        }
    }
}