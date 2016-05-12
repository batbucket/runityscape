using System;
using UnityEngine;
using UnityEngine.UI;

public class RightBoxView : AvatarBoxView {
    [SerializeField]
    Image avatar;

    public override void WriteText(Sprite sprite, TextBox textBox, Action callBack = null) {
        WriteText(avatar, sprite, textBox, callBack);
    }
}
