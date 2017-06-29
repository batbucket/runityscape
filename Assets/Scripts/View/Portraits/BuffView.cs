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
        private Text nameText;

        /// <summary>
        /// Duration of the buff, displayed under text
        /// </summary>
        [SerializeField]
        private Text durationText;

        /// <summary>
        /// Background of the buff
        /// </summary>
        [SerializeField]
        private Image background;

        /// <summary>
        /// Icon representing the buff
        /// </summary>
        [SerializeField]
        private Image image;

        [SerializeField]
        private Tip Tip;

        public void Setup(
            string name,
            string duration,
            Sprite icon,
            Color color,
            string body
            ) {
            nameText.text = name;
            durationText.text = duration;
            image.sprite = icon;

            Tip.Setup(
                icon,
                name,
                body
                );
        }

        public override void Reset() {
            Setup(
                string.Empty,
                string.Empty,
                null,
                Color.white,
                string.Empty
                );
            Tip.Reset();
        }
    }
}