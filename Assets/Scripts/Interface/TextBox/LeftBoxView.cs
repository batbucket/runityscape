using System;
using UnityEngine;
using UnityEngine.UI;

public class LeftBoxView : AvatarBoxView {
    [SerializeField]
    Image avatar;

    public override void WriteText(Sprite sprite, TextBox textBox, Action callBack) {
        WriteText(avatar, sprite, textBox, callBack);
    }
}
