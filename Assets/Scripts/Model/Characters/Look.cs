using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Characters {

    public class Look {
        public string Name;
        public Sprite Sprite;
        public string Check;
        public Color TextColor;

        private string suffix;

        public Look() {
            this.Name = "Default";
            this.Sprite = Util.LoadIcon("fox-head");
            this.Check = "Placeholder";
            this.TextColor = Color.white;
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
    }
}
