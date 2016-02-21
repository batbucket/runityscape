using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour {

    [SerializeField]
    Text tooltipText;
    public string Text { set { tooltipText.text = value; } }

    public void Clear() {
        tooltipText.text = "";
    }
}
