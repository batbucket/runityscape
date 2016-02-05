using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour {
    public static TooltipManager Instance { get; private set; }

    Text tooltipText;

    // Use this for initialization
    void Awake() {
        Instance = this;
        this.tooltipText = gameObject.GetComponent<Text>();
    }

    public void Set(string tooltip) {
        tooltipText.text = tooltip;
    }

    public void Clear() {
        tooltipText.text = "";
    }
}
