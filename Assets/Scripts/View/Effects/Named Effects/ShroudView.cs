using System;
using Scripts.View.ObjectPool;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Scripts.View.Effects {
    /// <summary>
    /// A solid color image that is used to block content.
    /// </summary>
    /// <seealso cref="Scripts.View.ObjectPool.PooledBehaviour" />
    public class ShroudView : PooledBehaviour {
        /// <summary>
        /// The image
        /// </summary>
        [SerializeField]
        private Image image;
        /// <summary>
        /// The rect t
        /// </summary>
        [SerializeField]
        private RectTransform rectT;

        /// <summary>
        /// Sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color {
            set {
                image.color = value;
            }
        }

        /// <summary>
        /// Sets the dimensions.
        /// </summary>
        /// <value>
        /// The dimensions.
        /// </value>
        public Vector2 Dimensions {
            set {
                rectT.rect.Set(0, 0, value.x, value.y);
            }
        }

        /// <summary>
        /// Reset the state of this MonoBehavior.
        /// </summary>
        public override void Reset() {
            image.color = Color.white;
        }
    }
}