using Scripts.Game.Serialized;
using Scripts.Model.Items;
using System;
using UnityEngine;

namespace Scripts.Game.Pages {
    public class Buy {
        public Func<Item> GetItem;
        public Func<Flags, bool> Condition;
        public Action<Flags> SetFlags;
        public int BasePrice;
        public string Name;
        public string SalesPitch;
        public Sprite Sprite;

        private string description;

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

        public Buy AddPitch(string s) {
            SalesPitch = string.Format("\"{0}\"", s);
            return this;
        }

        public Buy AddCondition(Func<Flags, bool> condition) {
            this.Condition = condition;
            return this;
        }

        public Buy AddSetFlags(Action<Flags> setFlags) {
            this.SetFlags = setFlags;
            return this;
        }
    }
}