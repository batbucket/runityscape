using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scripts.Game.Defined.Characters;

namespace Scripts.Model.Characters {

    public class Look : ISaveable<LookSave> {
        public string Name;

        protected Color textColor;
        protected string check;
        protected string tooltip;
        protected Breed breed;

        private string suffix;
        private Sprite sprite;

        private int revealCount;

        public Look() {
            this.Name = "Default";
            this.sprite = Util.GetSprite("fox-head");
            this.tooltip = string.Empty;
            this.check = string.Empty;
            this.breed = Breed.UNKNOWN;
            this.textColor = Color.white;
        }

        public int RevealCount {
            get {
                return revealCount;
            }
        }

        public Breed Breed {
            get {
                return breed;
            }
        }

        public Sprite Sprite {
            get {
                return sprite;
            }
            protected set {
                this.sprite = value;
            }
        }

        public string Tooltip {
            get {
                return tooltip;
            }
        }
        public Color TextColor {
            get {
                return textColor;
            }
        }

        public string DisplayName {
            get {
                return string.Format("{0}{1}", Name, string.IsNullOrEmpty(suffix) ? "" : string.Format(" {0}", suffix));
            }
        }

        public string Suffix {
            set {
                suffix = value;
            }
        }

        public void AddRevealStack() {
            revealCount++;
        }

        public void RemoveRevealStack() {
            revealCount--;
        }

        public override bool Equals(object obj) {
            var item = obj as Look;

            if (item == null) {
                return false;
            }

            return
                this.Name.Equals(item.Name)
                && this.textColor.Equals(item.textColor)
                && this.tooltip.Equals(item.tooltip)
                && this.breed.Equals(item.breed)
                && this.sprite.Equals(item.sprite)
                && this.revealCount.Equals(item.revealCount)
                && this.check.Equals(item.check);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public LookSave GetSaveObject() {
            return new LookSave(Name, sprite, textColor, check, tooltip, breed, revealCount);
        }

        public void InitFromSaveObject(LookSave saveObject) {
            this.Name = saveObject.Name;
            this.sprite = saveObject.Sprite;
            this.textColor = saveObject.TextColor;
            this.check = saveObject.Check;
            this.tooltip = saveObject.Tooltip;
            this.breed = saveObject.Breed;
            this.revealCount = saveObject.RevealCount;
        }
    }
}
