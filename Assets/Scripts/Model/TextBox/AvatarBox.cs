using Scripts.Model.Pages;
using Scripts.View;
using Scripts.View.TextBoxes;
using System;
using UnityEngine;

namespace Scripts.Model.TextBoxes {

    /// <summary>
    /// Represents a textbox with a character portrait window to one of the sides.
    /// </summary>
    public class AvatarBox : TextBox {
        protected const float TEXT_SPEED = 0.01f;

        public readonly Sprite Sprite;
        private readonly TextBoxType type;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="side">Which side of the page the portrait should be on.</param>
        /// <param name="sprite">Sprite to use on the box</param>
        /// <param name="color">Main color of the text</param>
        /// <param name="text">Dialogue to be spoken.</param>
        public AvatarBox(Side side,
                         Sprite sprite,
                         Color color,
                         string text)
                         : base(text, color, TextEffect.TYPE, "Blip_0", TEXT_SPEED) {
            this.Sprite = sprite;
            if (side == Side.LEFT) {
                type = TextBoxType.LEFT;
            } else {
                type = TextBoxType.RIGHT;
            }
        }

        /// <summary>
        /// Interface constructor for the lazy
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        /// <param name="side">The side.</param>
        /// <param name="text">The text.</param>
        public AvatarBox(IAvatarable avatar, Side side, string text) : this(side, avatar.Sprite, avatar.TextColor, text) {
        }

        /// <summary>
        /// Type determines how the box's contents will be written out.
        /// </summary>
        public override TextBoxType Type {
            get {
                return this.type;
            }
        }

        /// <summary>
        /// Setup the gameobject's image's sprite.
        /// </summary>
        /// <param name="textBoxPrefab">Reference to the avatarbox prefab object.</param>
        protected override void SetupHelper(GameObject textBoxPrefab) {
            textBoxPrefab.GetComponent<AvatarBoxView>().Sprite = Sprite;
        }
    }
}