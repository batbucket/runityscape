using Scripts.Presenter;
using Scripts.View.ObjectPool;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.Portraits {

    /// <summary>
    /// This class represents a Buff that is visible
    /// under a Portrait's resourceviews and image.
    /// </summary>
    public class BuffView : PooledBehaviour {

        /// <summary>
        /// Outline of the buff's box
        /// </summary>
        [SerializeField]
        private Outline outline;

        /// <summary>
        /// Text inside the buff's box
        /// </summary>
        [SerializeField]
        private Text text;

        /// <summary>
        /// Duration of the buff, displayed under text
        /// </summary>
        [SerializeField]
        private Text duration;

        /// <summary>
        /// Background of the buff
        /// </summary>
        [SerializeField]
        private Image image;

        public string Text { set { text.text = value; } }
        public string Duration { set { duration.text = value; } }

        /// <summary>
        /// Buff description on mouse hoverover
        /// </summary>
        private string tooltip;

        public string Tooltip {
            set {
                tooltip = value;
            }
        }

        public Color Color {
            set {
                text.color = value;
                outline.effectColor = value;
            }
        }

        public override void Reset() {
            Text = "";
            Duration = "";
            Color = Color.white;
            image.color = Color.black;
        }

        private void OnMouseOver() {
            Game.Instance.Tooltip.MouseText = tooltip;
        }

        private void OnMouseExit() {
            Game.Instance.Tooltip.MouseText = "";
        }
    }
}