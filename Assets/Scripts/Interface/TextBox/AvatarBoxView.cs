using UnityEngine;
using UnityEngine.UI;
using System;

public class AvatarBoxView : PooledBehaviour {
    [SerializeField]
    private TextBoxView textBoxView;
    [SerializeField]
    private Image avatar;

    public void WriteText(AvatarBox a, Action callBack = null) {
        avatar.sprite = Util.GetSprite(a.SpriteLoc);
        textBoxView.WriteText(a, callBack);
    }

    public override void Reset() {
        textBoxView.Reset();
        avatar.sprite = null;
    }
}
