using Scripts.Model;
using Scripts.Model.Interfaces;
using Scripts.Model.Tooltips;
using UnityEngine;

namespace Scripts.View.Tooltip {

    /// <summary>
    /// A tip is added to a gameobject
    /// when one wants to display a tooltip on hoverover.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class Tip : MonoBehaviour {
        /// <summary>
        /// The sprite
        /// </summary>
        private Sprite sprite;
        /// <summary>
        /// The title
        /// </summary>
        private string title;
        /// <summary>
        /// The body
        /// </summary>
        private string body;

        /// <summary>
        /// Gets or sets the sprite.
        /// </summary>
        /// <value>
        /// The sprite.
        /// </value>
        public Sprite Sprite {
            get {
                return sprite;
            }

            set {
                sprite = value;
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title {
            get {
                return title;
            }

            set {
                title = value;
            }
        }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public string Body {
            get {
                return body;
            }

            set {
                body = value;
            }
        }

        /// <summary>
        /// Setups the specified bundle.
        /// </summary>
        /// <param name="bundle">The bundle.</param>
        public void Setup(TooltipBundle bundle) {
            this.sprite = bundle.Sprite;
            this.title = bundle.Title;
            this.body = bundle.Text;
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset() {
            this.sprite = null;
            this.title = string.Empty;
            this.body = string.Empty;
        }
    }
}