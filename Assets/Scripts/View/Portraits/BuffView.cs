using System;
using Scripts.Presenter;
using Scripts.View.ObjectPool;
using Scripts.View.Tooltip;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Model.Tooltips;

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

        /// <summary>
        /// Setup the buff view from these fields.
        /// </summary>
        /// <param name="name">Name of the buff</param>
        /// <param name="duration">Duration of the buff</param>
        /// <param name="icon">Icon of the buff</param>
        /// <param name="color">Color of the text</param>
        /// <param name="body">Tooltip description of the buff</param>
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
            nameText.color = color;

            Tip.Setup(new TooltipBundle(icon, name, body));
        }

        /// <summary>
        /// Reset all fields.
        /// </summary>
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