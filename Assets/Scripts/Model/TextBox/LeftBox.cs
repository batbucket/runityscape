using UnityEngine;

namespace Scripts.Model.TextBoxes {

    /// <summary>
    /// Avatarbox with image on left.
    /// </summary>
    public class LeftBox : AvatarBox {

        public LeftBox(string spriteLoc,
                       string text,
                       Color color)
                       : base(spriteLoc, text, color, "Blip_0", TEXT_SPEED) { }

        public override TextBoxType Type { get { return TextBoxType.LEFT; } }
    }
}