using UnityEngine;
using UnityEngine.UI;

public class BuffView : MonoBehaviour {
    [SerializeField]
    Image _icon;
    Sprite IconSprite { get { return _icon.sprite; } set { _icon.sprite = value; } }
    Color IconColor { get { return _icon.color; } set { _icon.color = value; } }

    [SerializeField]
    Text _text;
    string Text { get { return _text.text; } set { _text.text = value; } }
}
