using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.View.Tooltip {

    public class Tip : MonoBehaviour {
        private Sprite sprite;
        private string title;
        private string body;

        public Sprite Sprite {
            get {
                return sprite;
            }

            set {
                sprite = value;
            }
        }

        public string Title {
            get {
                return title;
            }

            set {
                title = value;
            }
        }

        public string Body {
            get {
                return body;
            }

            set {
                body = value;
            }
        }
    }
}