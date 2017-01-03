using UnityEngine;

namespace Scripts.Model.TextBoxes {

    /// <summary>
    /// AvatarBox with the Portrait on the right.
    /// </summary>
    public class RightBox : AvatarBox {

        public RightBox(string spriteLoc,
                       string text,
                       Color color)
                       : base(spriteLoc, text, color, "Blip_0", 0f) {
        }

        public RightBox(string spriteLoc,
                   string text)
                   : base(spriteLoc, text, Color.white, "Blip_0", 0f) {
        }

        public override TextBoxType Type { get { return TextBoxType.RIGHT; } }
    }
}