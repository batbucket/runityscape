using Scripts.Presenter;
using Scripts.View.ObjectPool;
using Scripts.View.Tooltip;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.Portraits {

    /// <summary>
    /// This class represents a Buff that is visible
    /// under a Portrait's resourceviews and image.
    /// </summary>
    public class BuffView : PooledBehaviour {

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
        private Image background;

        /// <summary>
        /// Icon representing the buff
        /// </summary>
        [SerializeField]
        private Image icon;

        [SerializeField]
        private Tip Tip;

        public Tip Tooltip {
            get {
                return Tip;
            }
        }

        public string Text {
            set {
                text.text = value;
            }
        }
        public string Duration {
            set {
                duration.text = value;
            }
        }
        public Sprite Icon {
            set {
                icon.sprite = value;
            }
        }

        public Color Color {
            set {
                text.color = value;
            }
        }

        public override void Reset() {
            Text = "";
            Duration = "";
            Color = Color.white;
            background.color = Color.clear;
        }
    }
}