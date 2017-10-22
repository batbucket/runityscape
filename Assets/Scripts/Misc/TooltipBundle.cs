using Scripts.Model.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scripts.Model.Tooltips {
    /// <summary>
    /// Bundle passed to the tooltip manager.
    /// </summary>
    public class TooltipBundle {
        /// <summary>
        /// The sprite
        /// </summary>
        public Sprite Sprite;
        /// <summary>
        /// The title
        /// </summary>
        public string Title;
        /// <summary>
        /// The text
        /// </summary>
        public string Text;

        /// <summary>
        /// Initializes a new instance of the <see cref="TooltipBundle"/> class.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        public TooltipBundle(Sprite sprite, string title, string text) {
            this.Sprite = sprite;
            this.Title = title;
            this.Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TooltipBundle"/> class.
        /// </summary>
        public TooltipBundle() {
            this.Sprite = null;
            this.Title = string.Empty;
            this.Text = string.Empty;
        }

        /// <summary>
        /// Gets the tooltip.
        /// </summary>
        /// <value>
        /// The tooltip.
        /// </value>
        public TooltipBundle Tooltip {
            get {
                return this;
            }
        }
    }
}
