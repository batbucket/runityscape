using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour {
    Text tooltipText;

    // Use this for initialization
    void Awake() {
        this.tooltipText = gameObject.GetComponent<Text>();
    }

    public void Set(string tooltip) {
        tooltipText.text = tooltip;
    }

    public void Clear() {
        tooltipText.text = "";
    }
}
