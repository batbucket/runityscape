using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour {

    [SerializeField]
    private Text tooltipText;

    public string PageText;
    public string MouseText;

    private void Update() {
        if (PageText != null) {
            tooltipText.text = PageText;
        }
        if (!string.IsNullOrEmpty(MouseText)) {
            tooltipText.text = MouseText;
        }
    }
}
