using Scripts.View;
using Scripts.View.TextBoxes;
using System;
using UnityEngine;

namespace Scripts.Model.TextBoxes {

    /// <summary>
    /// Represents a textbox with a character portrait window to one of the sides.
    /// </summary>
    public abstract class AvatarBox : TextBox {
        public readonly string SpriteLoc;

        public AvatarBox(string spriteLoc,
                         string text,
                         Color color,
                         string soundLocation,
                         float timePerLetter)
                         : base(text, color, TextEffect.TYPE, "Blip_0", 0) {
            this.SpriteLoc = spriteLoc;
        }

        public override void Write(GameObject avatarBoxPrefab, Action callBack) {
            avatarBoxPrefab.GetComponent<AvatarBoxView>().WriteText(this, callBack);
        }
    }
}