using UnityEngine;
using System.Collections;

public class RightBox : AvatarBox {
    public override TextBoxType Type { get { return TextBoxType.RIGHT; } }

    public RightBox(string spriteLoc,
                   string text,
                   Color color,
                   TextEffect effect = TextEffect.TYPE,
                   string soundLocation = "Blip_0",
                   float timePerLetter = 0.075f)
                   : base(spriteLoc, text, color, effect, soundLocation, timePerLetter) {
    }

    public RightBox(Character c, string text) : base(c, text) { }
}
