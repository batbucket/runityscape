using Scripts.Model;
using Scripts.Model.Interfaces;
using Scripts.Model.Tooltips;
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

        public void Setup(TooltipBundle bundle) {
            this.sprite = bundle.Sprite;
            this.title = bundle.Title;
            this.body = bundle.Text;
        }

        public void Reset() {
            this.sprite = null;
            this.title = string.Empty;
            this.body = string.Empty;
        }
    }
}