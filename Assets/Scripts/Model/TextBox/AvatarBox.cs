using System;
using UnityEngine;

public abstract class AvatarBox : TextBox {

    string _spriteLoc;
    public string SpriteLoc { get { return _spriteLoc; } }

    public AvatarBox(string spriteLoc,
                     string text,
                     Color color,
                     TextEffect effect = TextEffect.TYPE,
                     string soundLocation = "Sounds/Blip_0",
                     float timePerLetter = 0.5f)
                     : base(text, color, effect, soundLocation, timePerLetter) {
        this._spriteLoc = spriteLoc;
    }

    public AvatarBox(Character c, string text) : base(text, c.TextColor, TextEffect.TYPE, timePerLetter: 0.075f) {
        this._spriteLoc = c.SpriteLoc;
    }

    public override void Write(GameObject avatarBoxPrefab, Action callBack) {
        avatarBoxPrefab.GetComponent<AvatarBoxView>().WriteText(this, callBack);
    }
}
