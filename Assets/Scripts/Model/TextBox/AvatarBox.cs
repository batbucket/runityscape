using System;
using UnityEngine;

public abstract class AvatarBox : TextBox {

    string _spriteLoc;
    public string SpriteLoc { get { return _spriteLoc; } }

    public AvatarBox(string spriteLoc,
                     string text,
                     Color color,
                     string soundLocation,
                     float timePerLetter)
                     : base(text, TextEffect.TYPE) {
        this._spriteLoc = spriteLoc;
    }

    public override void Write(GameObject avatarBoxPrefab, Action callBack) {
        avatarBoxPrefab.GetComponent<AvatarBoxView>().WriteText(this, callBack);
    }
}
