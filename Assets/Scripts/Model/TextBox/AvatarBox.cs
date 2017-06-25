using Scripts.View;
using Scripts.View.TextBoxes;
using System;
using UnityEngine;

namespace Scripts.Model.TextBoxes {
    public enum Side {
        LEFT,
        RIGHT
    }

    /// <summary>
    /// Represents a textbox with a character portrait window to one of the sides.
    /// </summary>
    public class AvatarBox : TextBox {
        protected const float TEXT_SPEED = 0.01f;

        public readonly Sprite Sprite;
        private readonly TextBoxType type;

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

        public override TextBoxType Type {
            get {
                return this.type;
            }
        }

        protected override void SetupHelper(GameObject textBoxPrefab) {
            textBoxPrefab.GetComponent<AvatarBoxView>().Sprite = Sprite;
        }
    }
}