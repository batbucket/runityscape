using UnityEngine;
using UnityEngine.UI;

public class BuffView : MonoBehaviour {
    [SerializeField]
    Image _icon;
    public Image Icon { get { return _icon; } set { _icon = value; } }

    [SerializeField]
    Text _text;
    public string Text { get { return _text.text; } set { _text.text = value; } }
}
