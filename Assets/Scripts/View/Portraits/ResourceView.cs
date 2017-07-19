using Scripts.Model.Stats;
using Scripts.View.ObjectPool;
using Scripts.View.Tooltip;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.Portraits {

    /// <summary>
    /// This class represents a bar and its display
    /// on a CharacterView, representing their resources.
    /// </summary>
    public class ResourceView : PooledBehaviour, IComparable<ResourceView> {

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

        private int order;

        private float BarScale {
            set {
                Vector3 v = fillBar.gameObject.GetComponent<RectTransform>().localScale;
                v.x = Mathf.Clamp(value, 0, 1);
                fillBar.gameObject.GetComponent<RectTransform>().localScale = v;
            }
        }

        public void Setup(
            Color negativeColor,
            Color fillColor,
            Sprite sprite,
            string barText,
            int numerator,
            int denominator,
            string title,
            string typeDescription) {
            emptyBar.color = negativeColor;
            fillBar.color = fillColor;
            resourceIcon.sprite = sprite;
            text.text = barText;
            SetBarScale(numerator, denominator);
            tip.Setup(sprite, title, string.Format("Current: {0}\nMaximum: {1}\n\n{2}", numerator, denominator, typeDescription));
        }

        public override void Reset() {
            Setup(
                Color.white,
                Color.white,
                null,
                string.Empty,
                0,
                0,
                string.Empty,
                string.Empty
                );
            text.color = Color.white;
        }

        private void SetBarScale(float numerator, float denominator) {
            if (denominator <= 0) {
                BarScale = 0;
            } else {
                BarScale = numerator / denominator;
            }
        }

        int IComparable<ResourceView>.CompareTo(ResourceView other) {
            return this.order.CompareTo(other.order);
        }
    }
}