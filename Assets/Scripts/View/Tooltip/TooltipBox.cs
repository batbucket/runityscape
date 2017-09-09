using Scripts.View.ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.View.Tooltip {

    /// <summary>
    /// Tooltip box for displaying tips
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class TooltipBox : MonoBehaviour {

        /// <summary>
        /// The image
        /// </summary>
        [SerializeField]
        private Image image;

        /// <summary>
        /// The title
        /// </summary>
        [SerializeField]
        private Text title;

        /// <summary>
        /// The body
        /// </summary>
        [SerializeField]
        private Text body;

        /// <summary>
        /// The backdrop
        /// </summary>
        [SerializeField]
        private RectTransform backdrop;

        /// <summary>
        /// The rt
        /// </summary>
        [SerializeField]
        private RectTransform rt;

        /// <summary>
        /// The outline
        /// </summary>
        [SerializeField]
        private Outline outline;

        /// <summary>
        /// Gets the outline.
        /// </summary>
        /// <value>
        /// The outline.
        /// </value>
        public Outline Outline {
            get {
                return outline;
            }
        }

        /// <summary>
        /// Sets the sprite.
        /// </summary>
        /// <value>
        /// The sprite.
        /// </value>
        public Sprite Sprite {
            set {
                image.sprite = value;
            }
        }

        /// <summary>
        /// Sets a value indicating whether this instance is icon enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is icon enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsIconEnabled {
            set {
                image.gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title {
            set {
                title.text = value;
            }
        }

        /// <summary>
        /// Sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public string Body {
            set {
                body.text = value;
            }
        }

        /// <summary>
        /// Sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector2 Position {
            set {
                transform.position = value;
            }
        }

        /// <summary>
        /// Sets the pivot.
        /// </summary>
        /// <value>
        /// The pivot.
        /// </value>
        public Vector2 Pivot {
            set {
                rt.pivot = value;
            }
        }
    }
}