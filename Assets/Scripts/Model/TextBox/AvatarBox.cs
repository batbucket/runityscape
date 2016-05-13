using UnityEngine;

public abstract class AvatarBox : TextBox {

    Sprite _sprite;
    public Sprite Sprite { get { return _sprite; } }

    public AvatarBox(string spriteLoc,
                     string text,
                     TextEffect effect = TextEffect.TYPE,
                     string soundLocation = "Sounds/Blip_0",
                     float timePerLetter = 0.5f)
                     : base(text, effect, soundLocation, timePerLetter) {
        this._sprite = Util.GetSprite(spriteLoc);
    }

    public override void Write(GameObject avatarBoxPrefab) {
        avatarBoxPrefab.GetComponent<AvatarBoxView>().WriteText(this);
    }
}
