using UnityEngine;

namespace Scripts.Model.TextBoxes {

    /// <summary>
    /// AvatarBox with the Portrait on the right.
    /// </summary>
    public class RightBox : AvatarBox {

        public RightBox(Sprite sprite,
                       string text,
                       Color color)
                       : base(sprite, text, color, "Blip_0", TEXT_SPEED) {
        }

        public override TextBoxType Type { get { return TextBoxType.RIGHT; } }
    }
}