using UnityEngine;
using UnityEngine.UI;
using System;

public class AvatarBoxView : MonoBehaviour {
    [SerializeField]
    TextBoxView textBoxView;
    [SerializeField]
    Image avatar;

    public void WriteText(AvatarBox a, Action callBack = null) {
        avatar.sprite = Util.GetSprite(a.SpriteLoc);
        textBoxView.WriteText(a, callBack);
    }
}
