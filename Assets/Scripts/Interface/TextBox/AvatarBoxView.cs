using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class AvatarBoxView : MonoBehaviour {
    [SerializeField]
    TextBoxView textBoxView;

    protected void WriteText(Image avatar, Sprite sprite, TextBox textBox, Action callBack = null) {
        avatar.sprite = sprite;
        textBoxView.WriteText(textBox, callBack);
    }

    public abstract void WriteText(Sprite sprite, TextBox textBox, Action callBack = null);
}
