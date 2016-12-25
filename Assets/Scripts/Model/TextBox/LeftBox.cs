using UnityEngine;
using System.Collections;

public class LeftBox : AvatarBox {
    public override TextBoxType Type { get { return TextBoxType.LEFT; } }

    public LeftBox(string spriteLoc,
                   string text,
                   Color color)
                   : base(spriteLoc, text, color, "Blip_0", 0f) { }
}
