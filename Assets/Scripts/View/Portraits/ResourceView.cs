using Scripts.Model.Stats.Resources;
using Scripts.Presenter;
using Scripts.View.ObjectPool;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.Portraits {

    /// <summary>
    /// This class represents a bar and its display
    /// on a CharacterView, representing their resources.
    /// </summary>
    public class ResourceView : PooledBehaviour {
        public ResourceType Type;

        /// <summary>
        /// Bar that represents the absence of a resource
        /// </summary>
        [SerializeField]
        private Image emptyBar;

        /// <summary>
        /// Bar that represents having a resource
        /// </summary>
        [SerializeField]
        private Image fillBar;

        [SerializeField]
        private Text resourceName; //Should be only 2 letters.

        [SerializeField]
        private Text text;

        private string tooltip;

        public float BarScale {
            set {
                Vector3 v = fillBar.gameObject.GetComponent<RectTransform>().localScale;
                v.x = Mathf.Clamp(value, 0, 1);
                fillBar.gameObject.GetComponent<RectTransform>().localScale = v;
            }
        }

        public Color EmptyColor {
            get {
                return emptyBar.color;
            }
            set {
                emptyBar.color = value;
            }
        }

        public Color FillColor {
            get {
                return fillBar.color;
            }
            set {
                fillBar.color = value;
            }
        }

        public Color ResourceColor {
            get {
                return resourceName.color;
            }
            set {
                resourceName.color = value;
            }
        }

        public string ResourceName {
            get {
                return resourceName.text;
            }
            set {
                resourceName.text = value;
            }
        }

        /// <summary>
        /// Text on bar that describes the amount in some way
        /// </summary>
        public string Text {
            get {
                return text.text;
            }
            set {
                text.text = value;
            }
        }

        public string Tooltip {
            set {
                tooltip = value;
            }
        }

        public override void Reset() {
            ResourceName = "";
            ResourceColor = Color.white;
            EmptyColor = Color.white;
            FillColor = Color.white;
            Text = "";
        }

        public void SetBarScale(float numerator, float denominator) {
            if (denominator <= 0) {
                BarScale = 0;
            } else {
                BarScale = numerator / denominator;
            }
        }

        protected override string HoverOverText {
            get {
                return tooltip;
            }
        }
    }
}