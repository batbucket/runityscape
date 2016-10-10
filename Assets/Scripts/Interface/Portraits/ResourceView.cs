using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

/**
 * This class manages the Resource prefab
 */
public class ResourceView : PooledBehaviour {

    public ResourceType Type;

    [SerializeField]
    private Text resourceName; //Name of the Resource. Should be only 2 letters.
    [SerializeField]
    private Image underBar; //Bar that represents max resource
    [SerializeField]
    private Image overBar; //Bar that represents current resource
    [SerializeField]
    private Text text; //Text on bar that describes the amount in some way

    public string ResourceName { get { return resourceName.text; } set { resourceName.text = value; } }
    public Color ResourceColor { get { return resourceName.color; } set { resourceName.color = value; } }

    public Color UnderColor { get { return underBar.color; } set { underBar.color = value; } }
    public Color OverColor { get { return overBar.color; } set { overBar.color = value; } }

    public string Text { get { return text.text; } set { text.text = value; } }

    /// <summary>
    /// Scale should be in the range [0, 1]
    /// This sets the OverBar's scale
    /// </summary>
    public float BarScale {
        set {
            Vector3 v = overBar.gameObject.GetComponent<RectTransform>().localScale;
            v.x = Mathf.Clamp(value, 0, 1);
            overBar.gameObject.GetComponent<RectTransform>().localScale = v;
        }
    }

    public override void Reset() {
        ResourceName = "";
        ResourceColor = Color.white;
        UnderColor = Color.white;
        OverColor = Color.white;
        Text = "";
    }

    /**
     * Scale should be in the range [0, 1]
     * This sets the OverBar's scale
     */
    public void SetBarScale(float a, float b) {
        Vector3 v = overBar.gameObject.GetComponent<RectTransform>().localScale;
        if (b == 0) {
            v.x = 0;
        } else {
            v.x = Mathf.Clamp(a / b, 0, 1);
        }
        overBar.gameObject.GetComponent<RectTransform>().localScale = v;
    }
}
