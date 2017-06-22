using Scripts.Model.SaveLoad;
using Scripts.Model.SaveLoad.SaveObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scripts.Game.Defined.Characters;

namespace Scripts.Model.Characters {

    public class Look : ISaveable<LookSave, LookSave> {
        public string Name;

        protected Color textColor;
        protected string check;
        protected string tooltip;
        protected Breed breed;

        private string suffix;
        private Sprite sprite;

        public Look() {
            this.Name = "Default";
            this.sprite = Util.GetSprite("fox-head");
            this.tooltip = string.Empty;
            this.check = string.Empty;
            this.breed = Breed.UNKNOWN;
            this.textColor = Color.white;
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

        public LookSave GetSaveObject() {
            return new LookSave(Name, sprite, textColor, check, tooltip, breed);
        }

        public void InitFromSaveObject(LookSave saveObject) {
            this.Name = saveObject.Name;
            this.sprite = saveObject.Sprite;
            this.textColor = saveObject.TextColor;
            this.check = saveObject.Check;
            this.tooltip = saveObject.Tooltip;
            this.breed = saveObject.Breed;
        }
    }
}
