using UnityEngine;
using System.Collections;

public class RightBox : AvatarBox {
    public override TextBoxType Type { get { return TextBoxType.RIGHT; } }

    public RightBox(string spriteLoc,
                   string text,
                   Color color)
                   : base(spriteLoc, text, color, "Blip_0", 0f) {
    }

    public RightBox(string spriteLoc,
               string text)
               : base(spriteLoc, text, Color.white, "Blip_0", 0f) {
    }
}
