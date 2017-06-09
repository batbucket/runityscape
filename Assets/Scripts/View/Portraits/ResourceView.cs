using Scripts.Model.Stats;
using Scripts.View.ObjectPool;
using Scripts.View.Tooltip;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.Portraits {

    /// <summary>
    /// This class represents a bar and its display
    /// on a CharacterView, representing their resources.
    /// </summary>
    public class ResourceView : PooledBehaviour {
        public StatType Type;

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
        private Image resourceIcon;

        [SerializeField]
        private Text text;

        [SerializeField]
        private Tip tip;

        public Tip Tip {
            get {
                return tip;
            }
        }

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

        public Sprite Sprite {
            set {
                resourceIcon.sprite = value;
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

        public override void Reset() {
            Sprite = null;
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
            tip.Body = string.Format("Current: {0}\nMaximum: {1}\n\n{2}", numerator, denominator, Type.Description);
        }
    }
}