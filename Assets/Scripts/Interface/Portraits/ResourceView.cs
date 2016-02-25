using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * This class manages the Resource prefab
 */
public class ResourceView : MonoBehaviour {

    [SerializeField]
    Text _resourceName; //Name of the Resource. Should be only 2 letters.
    public string ResourceName { get { return _resourceName.text; } set { _resourceName.text = value; } }
    public Color ResourceColor { get { return _resourceName.color; } set { _resourceName.color = value; } }

    [SerializeField]
    Image _underBar; //Bar that represents max resource
    public Color UnderColor { get { return _underBar.color; } set { _underBar.color = value; } }

    [SerializeField]
    Image _overBar; //Bar that represents current resource
    public Color OverColor { get { return _overBar.color; } set { _overBar.color = value; } }

    [SerializeField]
    Text _text; //Text on bar that describes the amount in some way
    public string Text { get { return _text.text; } set { _text.text = value; } }

    /**
     * Scale should be in the range [0, 1]
     * This sets the OverBar's scale
     */
    public void SetBarScale(float scale) {
        Vector3 v = _overBar.gameObject.GetComponent<RectTransform>().localScale;
        v.x = Mathf.Clamp(scale, 0, 1);
        _overBar.gameObject.GetComponent<RectTransform>().localScale = v;
    }

    /**
     * Scale should be in the range [0, 1]
     * This sets the OverBar's scale
     */
    public void SetBarScale(float a, float b) {
        Vector3 v = _overBar.gameObject.GetComponent<RectTransform>().localScale;
        if (b == 0) {
            v.x = 0;
        } else {
            v.x = Mathf.Clamp(a / b, 0, 1);
        }
        _overBar.gameObject.GetComponent<RectTransform>().localScale = v;
    }
}
