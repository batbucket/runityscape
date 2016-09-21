using UnityEngine;
using System.Collections;

public class LeftBox : AvatarBox {
    public override TextBoxType Type { get { return TextBoxType.LEFT; } }

    public LeftBox(string spriteLoc,
                   string text,
                   Color color,
                   TextEffect effect = TextEffect.TYPE,
                   string soundLocation = "Blip_0",
                   float timePerLetter = 0.025f)
                   : base(spriteLoc, text, color, effect, soundLocation, timePerLetter) {

    }
    public LeftBox(Character c, string text) : base(c, text) { }
}
