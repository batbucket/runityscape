using System;
using UnityEngine;

public abstract class AvatarBox : TextBox {

    Sprite _sprite;
    public Sprite Sprite { get { return _sprite; } }

    public AvatarBox(string spriteLoc,
                     string text,
                     Color color,
                     TextEffect effect = TextEffect.TYPE,
                     string soundLocation = "Sounds/Blip_0",
                     float timePerLetter = 0.5f)
                     : base(text, color, effect, soundLocation, timePerLetter) {
        this._sprite = Util.GetSprite(spriteLoc);
    }

    public AvatarBox(Character c, string text) : base(text, c.TextColor, TextEffect.TYPE, timePerLetter: 0.075f) {
        this._sprite = Util.GetSprite(c.SpriteLoc);
    }

    public override void Write(GameObject avatarBoxPrefab, Action callBack) {
        avatarBoxPrefab.GetComponent<AvatarBoxView>().WriteText(this, callBack);
    }
}
