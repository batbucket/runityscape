using Scripts.Model.Stats;
using Scripts.Model.Tooltips;
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
    /// <seealso cref="Scripts.View.ObjectPool.PooledBehaviour" />
    /// <seealso cref="System.IComparable{Scripts.View.Portraits.ResourceView}" />
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

        /// <summary>
        /// The resource icon
        /// </summary>
        [SerializeField]
        private Image resourceIcon;

        /// <summary>
        /// The text
        /// </summary>
        [SerializeField]
        private Text text;

        /// <summary>
        /// The tip
        /// </summary>
        [SerializeField]
        private Tip tip;

        /// <summary>
        /// The order
        /// </summary>
        private int order;

        /// <summary>
        /// Sets the bar scale.
        /// </summary>
        /// <value>
        /// The bar scale.
        /// </value>
        private float BarScale {
            set {
                Vector3 v = fillBar.gameObject.GetComponent<RectTransform>().localScale;
                v.x = Mathf.Clamp(value, 0, 1);
                fillBar.gameObject.GetComponent<RectTransform>().localScale = v;
            }
        }

        /// <summary>
        /// Setups the specified negative color.
        /// </summary>
        /// <param name="negativeColor">Color of the negative.</param>
        /// <param name="fillColor">Color of the fill.</param>
        /// <param name="sprite">The sprite.</param>
        /// <param name="barText">The bar text.</param>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        /// <param name="title">The title.</param>
        /// <param name="typeDescription">The type description.</param>
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
            tip.Setup(new TooltipBundle(sprite, title, string.Format("Current: {0}\nMaximum: {1}\n\n{2}", numerator, denominator, typeDescription)));
        }

        /// <summary>
        /// Reset the state of this MonoBehavior.
        /// </summary>
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

        /// <summary>
        /// Sets the bar scale.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        private void SetBarScale(float numerator, float denominator) {
            if (denominator <= 0) {
                BarScale = 0;
            } else {
                BarScale = numerator / denominator;
            }
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        int IComparable<ResourceView>.CompareTo(ResourceView other) {
            return this.order.CompareTo(other.order);
        }
    }
}